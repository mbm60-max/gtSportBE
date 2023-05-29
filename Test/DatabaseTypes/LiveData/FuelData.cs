        public class FuelData{


        /// <summary>
        /// Gas level for the current car (in liters, from 0 to <see cref="GasCapacity"/>).
        /// <para> Note: This may change from 0 when regenerative braking with electric cars, check accordingly with <see cref="GasCapacity"/>. </para>
        /// </summary>
        public float GasLevel { get; set; }

        /// <summary>
        /// Max gas capacity for the current car.
        /// Will be 100 for most cars, 5 for karts, 0 for electric cars
        /// </summary>
        public float GasCapacity { get; set; }
        }