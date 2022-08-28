namespace TwitchLib.EventSub.Webhooks.Core.Models.Charity
{
    /// <summary>
    /// An object that contains the amount of charity related things.
    /// </summary>
    public class CharityAmount
    {
        /// <summary>
        /// The monetary amount. The amount is specified in the currency’s minor unit. For example, the minor units for USD is cents, so if the amount is $5.50 USD, value is set to 550.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// The number of decimal places used by the currency. For example, USD uses two decimal places. Use this number to translate value from minor units to major units by using the formula:
        /// <para>value / 10^decimal_places</para>
        /// </summary>
        public int DecimalPlaces { get; set; }
    }
}