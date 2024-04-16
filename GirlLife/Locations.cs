namespace GirlLife {
    public static class Locations {
        public static void CreateLocations() {
            new Location() {
                Name = "My Room",
                Body = "Just my room"
            }.Register("myRoom");
        }
    }
}
