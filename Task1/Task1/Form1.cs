using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task1
{
    //DESKTOP-NGARN3U\SQLEXPRESS
    public partial class Form1 : Form
    {
        private void gMapControl1_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            double lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
            double lng = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;
            Console.WriteLine(String.Format("Marker {0} was clicked.", lat));
            Console.WriteLine(String.Format("Marker {0} was clicked.", lng));
        }

        //Drag and Drop
        private bool _up = false;
        private GMapMarker _heldMarker;
        private bool _stillHolding = false;
        //  private double _deadzoneLat = 0;
        // private double _deadzoneLng = 0;
        private void gMapControl1_OnMarkerEnter(GMapMarker item)
        {
            _heldMarker = item;
        }
        private void gMapControl1_OnMarkerLeave(GMapMarker item)
        {
            _heldMarker = item;
            if (_up == true)
            {
                Console.WriteLine("XD");
            }
            else _heldMarker = null;
            
        }
        private void gMapControl1_OnMapUp(object sender, MouseEventArgs e)
        {
            _up = false;
            double lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
            double lng = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;
            Console.WriteLine(lat);
            Console.WriteLine(lng);
            if ((_heldMarker != null) && (_stillHolding))
            {
                _heldMarker.Position = new PointLatLng(lat, lng);
                dB.UpdateMarkerPosition(_heldMarker);
            }
            _s_cts.Cancel();
            _stillHolding = false;
            Form.ActiveForm.Text = Form.ActiveForm.Text.Replace("(Holding)", "");
        }
        private bool asRunn = false;
        private async Task IsHolding(CancellationToken c)
        {
            asRunn = true;
            await Task.Delay(150);
            asRunn = false;
            Console.WriteLine("PRIkOL");
            if (!c.IsCancellationRequested)
            {
                Console.WriteLine("donne");
                _stillHolding = true;
                Form.ActiveForm.Text += "(Holding)";
            }
        }

        private CancellationTokenSource _s_cts = new CancellationTokenSource();
        private void gMapControl1_OnMapDown(object sender, MouseEventArgs e)
        {
            CancellationTokenSource s_cts = new CancellationTokenSource();
            _s_cts.Dispose();
            _s_cts = s_cts;
            _up = true;
            if (!asRunn)
            {
               var xd = IsHolding(_s_cts.Token);
            }
            else
            {
                Form.ActiveForm.Text = Form.ActiveForm.Text.Replace("(Holding)", "");
                _s_cts.Cancel();
            }
            double lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
            double lng = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;
            Console.WriteLine("XD"+lat);
            Console.WriteLine("XD"+lng);

        }
        private void gMapControl1_OnZoomChange()
        {
            //CalculateDeadZone();
        }
        //private void CalculateDeadZone()
        //{
        //    //0.00000538 В одну еденицу размера gmap элемента помещается координат при 18ом зуме
        //    double onePixCoord = 0.00000538;
        //    //TODO По-хорошему сделать в другом месте
        //    if (gMapControl1.MaxZoom > 18)
        //    {
        //        int counter = 0;
        //        for (int i = gMapControl1.MaxZoom; i != 18; i--)
        //        {
        //            counter++;
        //        }
        //        onePixCoord /= (2 ^ counter);
        //    }
        //    double j = gMapControl1.MaxZoom - gMapControl1.Zoom;
        //    //25x34 примерные размеры маркера
        //    _deadzoneLat = onePixCoord * Math.Pow(2, j) * 34;
        //    _deadzoneLng = onePixCoord * Math.Pow(2, j) * 25;
        //}

        public Form1()
        {
            InitializeComponent();
        }
        DBOperations dB = new DBOperations();
        private void Form1_Load(object sender, EventArgs e)
        {
            this.gMapControl1.MapProvider = GoogleMapProvider.Instance; //  Set the map source
            GMaps.Instance.Mode = AccessMode.ServerOnly; //  GMAP working mode
            this.gMapControl1.SetPositionByKeywords("Novosibirsk"); //  Map center location
            this.gMapControl1.Position = new PointLatLng(55.018803, 82.933952);
            this.gMapControl1.ShowCenter = false;
            gMapControl1.OnMarkerClick += new MarkerClick(gMapControl1_OnMarkerClick);
            gMapControl1.MouseUp += new MouseEventHandler(gMapControl1_OnMapUp);
            gMapControl1.MouseDown += new MouseEventHandler(gMapControl1_OnMapDown);
            gMapControl1.OnMarkerEnter += new MarkerEnter(gMapControl1_OnMarkerEnter);
            gMapControl1.OnMarkerLeave += new MarkerLeave(gMapControl1_OnMarkerLeave);
            gMapControl1.OnMapZoomChanged += new MapZoomChanged(gMapControl1_OnZoomChange);
            //Create a layer named "Markers"
            GMapOverlay markers = new GMapOverlay("markers");
            //CalculateDeadZone();
            MarkerSetuper marker = new MarkerSetuper();
            dB.Connect();
            List<DBMarker> dbRes = dB.GetData();
            foreach (DBMarker i in dbRes)
            {
                marker.SetMarker(markers, this.gMapControl1,i);
            }
        }

        private void ActiveForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dB.Disconnect();
        }
    }

    class MarkerSetuper
    { 
        public void SetMarker(GMapOverlay overlay,GMapControl gm,DBMarker dB)
        {
            //Create tags and set location and style
                GMapMarker marker = new GMarkerGoogle(new PointLatLng(dB.Latitude, dB.Longtitude), GMarkerGoogleType.red_dot);
                //Add the tag to the layer
                overlay.Markers.Add(marker);
                //Add the layer to the map
                gm.Overlays.Add(overlay);
                //marker properties
                marker.Tag = dB.ID;
                marker.ToolTipText = dB.Name;
                marker.ToolTip.Fill = new SolidBrush(Color.FromArgb(100, Color.Black));
                marker.ToolTip.Foreground = Brushes.White;
                marker.ToolTip.TextPadding = new Size(20, 20);
        }

        
    }
    class DBMarker
    {
        public int ID { get; set; }
        public float Latitude { get; set; }
        public float Longtitude { get; set; }
        public string Name { get; set; }

    }
    class DBOperations
    {
        private SqlConnection cnn;
        public void Connect()
        {
            const string connectionString = "Data Source=DESKTOP-NGARN3U\\SQLEXPRESS;Integrated Security=True;Initial Catalog=task1;User ID=oleg;Password=1";
            cnn = new SqlConnection(connectionString);
            cnn.Open();
        }
        public void Disconnect()
        {
            cnn.Dispose();
        }
        public List<DBMarker> GetData()
        {
            string q = "SELECT * FROM Markers";
            List<DBMarker> outp = new List<DBMarker>();
            SqlCommand com = new SqlCommand(q,cnn);
            SqlDataReader dataReader = com.ExecuteReader();
            while (dataReader.Read())
            {
                DBMarker d = new DBMarker();
                d.ID = (int)dataReader.GetValue(0);
                d.Latitude = float.Parse(dataReader.GetValue(1).ToString());
                d.Longtitude = float.Parse(dataReader.GetValue(2).ToString());
                d.Name = dataReader.GetValue(3).ToString();
                outp.Add(d);
            }
            dataReader.Close();
            com.Dispose();
            return outp;
        }
        public void UpdateMarkerPosition(GMapMarker item) 
        {
            string q = "UPDATE Markers set Latitude=" + item.Position.Lat.ToString().Replace(",", ".") + ",Longtitude=" + item.Position.Lng.ToString().Replace(",", ".") + " WHERE ID=" + item.Tag.ToString();
            SqlCommand comm = new SqlCommand(q, cnn);
            comm.ExecuteNonQuery();
            comm.Dispose();
        }
    }
}
