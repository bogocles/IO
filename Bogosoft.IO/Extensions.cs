using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.IO
{
    /// <summary>
    /// Contains static methods for working with text readers.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Read a line of characters asynchronously and returns the data as a string.
        /// </summary>
        /// <param name="reader">The current text reader.</param>
        /// <param name="token">An opportunity to respond to a cancellation request.</param>
        /// <returns>
        /// The next line from the current text reader, or null if all characters have been read.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Throw in the event that the current reader is null.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// Thrown in the event that the given token represents a cancelled request.
        /// </exception>
        public static Task<string> ReadLineAsync(this TextReader reader, CancellationToken token)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            token.ThrowIfCancellationRequested();

            return reader.ReadLineAsync();
        }

        /// <summary>
        /// Read all lines from the current text reader until no more lines remain.
        /// </summary>
        /// <param name="reader">The current text reader.</param>
        /// <param name="token">
        /// An opportunity to respond to a cancellation request. If cancellation has been requested,
        /// line iteration stops.
        /// </param>
        /// <returns>A sequence of lines obtained from the current text reader.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown in the event that the current reader is null.
        /// </exception>
        public static IEnumerable<string> ReadLines(this TextReader reader, CancellationToken token = default)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            while (!token.IsCancellationRequested && reader.TryReadLine(out string line))
            {
                yield return line;
            }
        }

        /// <summary>
        /// Asynchronously read all lines from the current text reader until no more lines remain.
        /// </summary>
        /// <param name="reader">The current text reader.</param>
        /// <param name="token">
        /// An opportunity to respond to a cancellation request. If cancellation has been requested,
        /// line iteration stops.
        /// </param>
        /// <returns>A sequence of lines obtained from the current text reader.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown in the event that the current reader is null.
        /// </exception>
        public static async IAsyncEnumerable<string> ReadLinesAsync(
            this TextReader reader,
            CancellationToken token = default
            )
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            string line;

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                if ((line = await reader.ReadLineAsync().ConfigureAwait(false)) is null)
                {
                    break;
                }

                yield return line;
            }
        }

        /// <summary>
        /// Attempt to read the next line from the current reader.
        /// </summary>
        /// <param name="reader">The current text reader.</param>
        /// <param name="line">The result of the read operation.</param>
        /// <returns>
        /// A value indicating whether or not the read operation succeeded.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown in the event that the current reader is null.
        /// </exception>
        public static bool TryReadLine(this TextReader reader, out string line)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return (line = reader.ReadLine()) != null;
        }
    }
}