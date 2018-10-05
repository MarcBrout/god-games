using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsBuffer<T>
{
    protected class InputBuffer
    {
        public T Input { get; set; }
        public float PressedAt { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="input">The key pressed</param>
        /// <param name="bufferDuration">The duration during which the key is considered pressed</param>
        public InputBuffer(T input)
        {
            Input = input;
            PressedAt = Time.time;
        }

        /// <summary>
        /// Is the input still buffred
        /// </summary>
        /// <param name="input"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsInputBuffered(T input, float bufferedTime)
        {
            return Input.Equals(input) && Time.time - PressedAt <= bufferedTime;
        }

        /// <summary>
        /// Is the input not considered as buffered anymore 
        /// </summary>
        /// <returns></returns>
        public bool IsOldInput(float bufferedTime)
        {
            return Time.time - PressedAt >= bufferedTime;
        }
    }

    protected List<InputBuffer> BufferedInputList { get; set; }
    protected float autoCleanBufferedInputDelay = 1f;
    protected int autoCleanBufferedInputLimit = 100;

    private const float standarBufferDuration = 0.1f;

    /// <summary>
    /// Constructor
    /// </summary>
    public InputsBuffer()
    {
        BufferedInputList = new List<InputBuffer>();
    }

    /// <summary>
    /// Add an input to the buffered stack
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pressedAt"></param>
    /// <param name="bufferDuration"></param>
    public void AddInput(T input)
    {
        BufferedInputList.Add(new InputBuffer(input));
    }

    /// <summary>
    /// Check if an input is still valid in the buffered stack
    /// </summary>
    /// <param name="input"></param>
    /// <param name="bufferedTime"></param>
    /// <param name="autoCLeanBufferedInput"></param>
    /// <returns></returns>
    public bool IsInputBuffered(T input, float bufferedTime = standarBufferDuration, bool autoCLeanBufferedInput = true)
    {
        if (autoCLeanBufferedInput && BufferedInputList.Count > autoCleanBufferedInputLimit)
            CleanBufferedInput(autoCleanBufferedInputDelay);
        return BufferedInputList.Find(x => x.IsInputBuffered(input, bufferedTime)) != null;
    }

    /// <summary>
    /// Remove all inputs not considered as buffered anymore
    /// </summary>
    /// <param name="bufferedTime"></param>
    public void CleanBufferedInput(float bufferedTime = standarBufferDuration)
    {
        BufferedInputList.RemoveAll(x => x.IsOldInput(bufferedTime));
    }

}



