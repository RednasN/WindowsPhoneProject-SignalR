namespace WPSignalR
{
    public class Location
    {
        public Location(string userId, double latitude, double longitude)
        {
            this.userId = userId;
            this.latitude = latitude;
            this.longitude = longitude;
        }
        public string userId { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
