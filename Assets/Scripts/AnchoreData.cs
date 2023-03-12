
public struct AutoHight
{
    bool autoHight;

}
public class AnchoreData
{



    public string Title { get; set; }
    public string Description { get; set; }
    public string FullDiscription { get; set; }
    public double Latitude { get; set; }

    public double Longitude { get; set; }
    public double Altitude { get; set; }

    public double Heading { get; set; }

    public float Qua_Z { get; set; }

    public float Qua_Y { get; set; }

    public float Qua_x { get; set; }

    public string URL { get; set; }

    public AnchoreData(string Title, string Discription, string FullDiscription, double Latitude
                      , double Longitude, double Altitude, double Heading, float Qua_x, float Qua_Y,
                      float Qua_Z, string URL)
    {
        this.Title = Title;
        this.Description = Discription;
        this.FullDiscription = FullDiscription;
        this.Latitude = Latitude;
        this.Longitude = Longitude;
        this.Altitude = Altitude;
        this.Heading = Heading;
        this.Qua_x = Qua_x;
        this.Qua_Y = Qua_Y;
        this.Qua_Z = Qua_Z;
        this.URL = URL;

    }

    public AnchoreData() { }




}
