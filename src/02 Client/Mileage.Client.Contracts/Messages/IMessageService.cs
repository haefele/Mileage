namespace Mileage.Client.Contracts.Messages
{
    public interface IMessageService
    {
        /// <summary>
        /// Creates and shows a dialog with the specified <paramref name="message"/>, <paramref name="caption"/>, <paramref name="image"/> and <paramref name="buttons"/>.
        /// Returns the index of the clicked button (0 = first button, 1 = second button, ...).
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="image">The image.</param>
        /// <param name="buttons">The buttons.</param>
        int ShowDialog(string message, string caption, MessageImage image, params string[] buttons);
    }
}