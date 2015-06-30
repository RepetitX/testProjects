using System;

namespace Common.Web.Mvc
{
    public static class GeoHelper
    {
        const int EarthRadius = 6372797; // Средний радиус Земли в метрах.

        /// <summary>
        /// Рассчитывает расстояние между двумя географическими точками.
        /// </summary>
        /// <param name="latitude1">Широта первой точки.</param>
        /// <param name="longitude1">Долгота первой точки.</param>
        /// <param name="latitude2">Широта второй точки.</param>
        /// <param name="longitude2">Долгота второй точки.</param>
        /// <returns>Расстояние между двумя географическими точками.</returns>
        public static double GetDistanceBetweenTwoPoints(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var lat = ConvertDegreesToRadians(latitude2 - latitude1);
            var lon = ConvertDegreesToRadians(longitude2 - longitude1);

            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                Math.Cos(ConvertDegreesToRadians(latitude1)) * Math.Cos(ConvertDegreesToRadians(latitude2)) *
                Math.Sin(lon / 2) * Math.Sin(lon / 2);

            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));

            var distance = Math.Round((EarthRadius * h2) / 1000, 1);

            return distance;
        }

        /// <summary>
        /// Рассчитывает координаты квадрата на основе текущего местоположения.
        /// </summary>
        /// <param name="latitude">Широта определенного местоположения.</param>
        /// <param name="longitude">Долгота определенного местоположения.</param>
        /// <param name="accuracy">Точность определенного местоположения в метрах.</param>
        /// <returns>Координаты квадрата (левого нижнего и правого верхнего углов).</returns>
        public static double[,] GetSquareByLocation(double latitude, double longitude, double accuracy)
        {
            // Для расчета квадрата нужно переводить значение точности (accuracy) из метров в градусы.
            // Рассчитываем длину одного градуса широты и долготы в метрах.
            var latitudeLength = 2 * Math.PI * EarthRadius / 360; // 2*Pi*R/360
            var longitudeLength = 2 * Math.PI * EarthRadius * Math.Cos(ConvertDegreesToRadians(latitude)) / 360; // 2*Pi*R*Cos(Fi)/360

            // Высчитываем точность (accuracy) местоположения в географических координатах (градусах).
            var latitudeAccuracy = accuracy / latitudeLength;
            var longitudeAccuracy = accuracy / longitudeLength;

            // Возвращаем двумерный массив с координатами квадрата поиска: левый нижний и правый верхний угол
            return new double[2, 2] { { (latitude - latitudeAccuracy), (longitude - longitudeAccuracy) }, { (latitude + latitudeAccuracy), (longitude + longitudeAccuracy) } };
        }

        /// <summary>
        /// Переводит градусы в радианы.
        /// </summary>
        /// <param name="deg">Величина угла в градусах.</param>
        /// <returns>Величина в радианах.</returns>
        public static double ConvertDegreesToRadians(double deg)
        {
            return (Math.PI / 180) * deg;
        }

        /// <summary>
        /// Переводит радианы в градусы.
        /// </summary>
        /// <param name="rad">Величина угла в радианах.</param>
        /// <returns>Величина в градусах.</returns>
        public static double ConvertRadiansToDegrees(double rad)
        {
            return (180 / Math.PI) * rad;
        }
    }
}
