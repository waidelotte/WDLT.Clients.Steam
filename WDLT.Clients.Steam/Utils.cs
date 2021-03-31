using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using WDLT.Clients.Steam.Enums;
using WDLT.Clients.Steam.Models;

namespace WDLT.Clients.Steam
{
    public static class Utils
    {
        public static string HashNameFromUrl(this string url)
        {
            var urlSplit = url.Split('/');
            return urlSplit.Last().Split('?')[0];
        }

        public static EApp AppFromUrl(this string url)
        {
            var urlSplit = url.Split('/');
            return (EApp)int.Parse(urlSplit[^2]);
        }

        public static double PriceCorrection(this double price, int walletFeePercent, int publisherFeePercent, double feeMinimum, double minPriceForOrder)
        {
            if (price <= minPriceForOrder) return minPriceForOrder;

            var tempPrice = price;
            while (tempPrice.PriceReFee(walletFeePercent, publisherFeePercent, feeMinimum) > price)
            {
                tempPrice -= 0.01;
            }

            return tempPrice;
        }

        public static double PriceReFee(this double price, int walletFeePercent, int publisherFeePercent, double feeMinimum)
        {
            var priseWithoutFee = PriseWithoutFee(price, walletFeePercent, publisherFeePercent, feeMinimum);
            return PriseWithFee(priseWithoutFee, walletFeePercent, publisherFeePercent, feeMinimum);
        }

        public static double PriseWithoutFee(this double price, UserWallet wallet)
        {
            return PriseWithoutFee(price, wallet.FeePercent, wallet.DefaultPublisherFeePercent, wallet.MinimumFee);
        }

        public static double PriseWithoutFee(this decimal price, int walletFeePercent, int publisherFeePercent, double feeMinimum)
        {
            return PriseWithoutFee((double)price, walletFeePercent, publisherFeePercent, feeMinimum);
        }

        public static double PriseWithoutFee(this double price, long walletFeePercent, long publisherFeePercent, double feeMinimum)
        {
            var tempPrice = 100 * price;
            var totalFee = 100 + walletFeePercent + publisherFeePercent;

            var publisherFee = Math.Max(Math.Floor(tempPrice / totalFee * publisherFeePercent), feeMinimum * 100);
            var steamFee = Math.Max(Math.Floor(tempPrice / totalFee * walletFeePercent), feeMinimum * 100);

            return Math.Max(Math.Round((tempPrice - (publisherFee + steamFee)) / 100, 2), feeMinimum);
        }

        public static double PriseWithFee(this double price, int walletFeePercent, int publisherFeePercent, double feeMinimum)
        {
            var tempPrice = price * 100;
            var tempFeeMinimum = feeMinimum * 100;
            var tempPublisherFeePercent = (double)publisherFeePercent / 100;
            var tempWalletFeePercent = (double)walletFeePercent / 100;

            var priseWithFee = Math.Max(tempPrice, tempFeeMinimum) + Math.Max(Math.Floor(tempPublisherFeePercent * tempPrice), tempFeeMinimum) + Math.Max(Math.Floor(tempWalletFeePercent * tempPrice), tempFeeMinimum);
            return priseWithFee / 100;
        }

        public static double ParseSteamCurrency(this string value)
        {
            var match = Regex.Match(value.Trim(), @"[0-9]{1,}((\.|\,)[0-9]{1,2})?").Value.Replace(",", ".");
            return double.Parse(match, CultureInfo.InvariantCulture);
        }
    }
}