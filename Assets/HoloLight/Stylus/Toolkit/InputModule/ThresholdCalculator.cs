#region copyright
/*******************************************************
 * Copyright (C) 2017-2018 Holo-Light GmbH -> <info@holo-light.com>
 * 
 * This file is part of the Stylus SDK.
 * 
 * Stylus SDK can not be copied and/or distributed without the express
 * permission of the Holo-Light GmbH
 * 
 * Author of this file is Peter Roth
 *******************************************************/
#endregion
using System.Collections.Generic;
using HoloLight.HoloStylus.Configuration;

namespace HoloLight.HoloStylus.InputModule
{
    /// <summary>
    /// States of ThresholdCalculator.
    /// </summary>
    public enum ThresholdStates
    {
        NotInitialized,
        WithinThreshold,
        OutsideFluently
    };

    /// <summary>
    /// Base class for threshold calculations.
    /// Input values are only added if a certain threshold is exceeded.  
    /// </summary>
    /// <typeparam name="T">Generic parameter for input data.</typeparam>
    public abstract class ThresholdCalculator<T> where T : IStylusCheckLimit<T>
    {
        protected readonly List<T> ValueHistory = new List<T>();
        protected readonly Queue<T> FluentHistory = new Queue<T>();

        protected readonly int NumberOfRelevantValues;

        protected readonly Thresholds Thresholds;
        protected ThresholdStates CurrentState = ThresholdStates.NotInitialized;

        protected T FirstHistoryData
        {
            get { return this.ValueHistory[0]; }
        }

        protected ThresholdCalculator(int numberOfRelevantValues, Thresholds thresholds)
        {
            NumberOfRelevantValues = numberOfRelevantValues;
            this.Thresholds = thresholds.CreateCalculatedThresholds();
        }

        /// <summary>
        /// Add a value to history after threshold checking.
        /// </summary>
        /// <param name="inputData">Input value. Threshold is checked and added to history if threshold is exceeded.</param>
        /// <returns>The selected threshold value.</returns>
        public T AddValue(T inputData)
        {
            T result = inputData;

            switch (CurrentState)
            {
                case ThresholdStates.NotInitialized:
                    // add values unchecked as long as not initialized
                    this.ValueHistory.Add(inputData);

                    if (IsInitialized)
                    {
                        ChangeState(ThresholdStates.WithinThreshold);
                    }

                    result = this.FirstHistoryData;

                    break;
                case ThresholdStates.WithinThreshold:
                    bool isInside = IsNonFluentValueWithinThreshold(inputData);

                    // always return first value when inside threshold
                    result = isInside ? this.FirstHistoryData : inputData;

                    if (!isInside)
                    {
                        this.FluentHistory.Clear();
                        ChangeState(ThresholdStates.OutsideFluently);
                    }

                    break;
                case ThresholdStates.OutsideFluently:
                    result = inputData;

                    // add to fluent list
                    AddFluentValue(inputData);

                    bool shouldRestart = IsFluentValueWithinThreshold(inputData);

                    if (shouldRestart)
                    {
                        this.ValueHistory.Clear();
                        ChangeState(ThresholdStates.NotInitialized);
                    }

                    break;
            }

            return result;
        }

        /// <summary>
        /// True if a minimum of NumberOfRelevantValues were added to value history.
        /// </summary>
        public bool IsInitialized
        {
            get { return this.ValueHistory.Count >= this.NumberOfRelevantValues; }
        }

        protected virtual bool IsNonFluentValueWithinThreshold(T inputData)
        {
            return !this.FirstHistoryData.IsOutsideLimits(inputData, this.Thresholds);
        }

        protected virtual bool IsFluentValueWithinThreshold(T inputData)
        {
            // get Value from FluentHistory
            T oldestValue = this.FluentHistory.Peek();

            bool isBackToLimits = oldestValue.IsOutsideLimits(inputData, this.Thresholds);
            return isBackToLimits;
        }

        protected void AddFluentValue(T inputData)
        {
            if (this.FluentHistory.Count >= this.NumberOfRelevantValues)
            {
                // kick the first value, not needed
                this.FluentHistory.Dequeue();
            }

            this.FluentHistory.Enqueue(inputData);
        }

        /// <summary>
        /// Change state of ThresholdCalculator.
        /// </summary>
        protected void ChangeState(ThresholdStates newState)
        {
            this.CurrentState = newState;
        }
    }
}