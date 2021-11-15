using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace ActTec
{
    interface BGW
    {
        void BackGroundWorker_start();
        void BackGroundWorker_stop();
        void reset_timer();
    }
    internal class BackGroundWorker : BGW
    {
        bool worker_life = false;
        Thread worker;
        update_delegate update;
        UInt32 time_sec = 0;
        public BackGroundWorker(update_delegate update)
        {
            this.update = update;
            worker = new Thread(WORK);
        }
        public void BackGroundWorker_start()
        {
            worker_life = true;
            worker.Start();
        }
        public void reset_timer()
        {
            time_sec = 0;
        }
        public void BackGroundWorker_stop()
        {
            worker_life = false;
        }

        void WORK()
        {
            pattern_site ps;
            List<pattern_site> sites = new List<pattern_site>();
            while (worker_life)
            {
                sites.Clear();
                foreach (string site in Properties.Settings.Default.List_url_sites)
                {
                    ps = new pattern_site();
                    ps.name = site.Split('|')[0];
                    ps.url = site.Split('|')[1];
                    ps.time = Convert.ToInt32(site.Split('|')[2]);
                    sites.Add(ps);
                }
                foreach (pattern_site site in sites)
                {
                    if (time_sec % site.time == 0)
                    {
                        Update_data_site(site);
                    }
                }
                time_sec++;
                Thread.Sleep(1000);
            }
        }
        void Update_data_site(pattern_site site)
        {
            int[] status_is_OK = new int[] { 200, 202, 301, 302 };
            for (int i = 0; i < Properties.Settings.Default.List_url_sites.Count; i++)
            {
                if (Properties.Settings.Default.List_url_sites[i] == site.name + "|" + site.url + "|" + site.time)
                {
                    HttpWebRequest httpReq = null;
                    HttpWebResponse httpRes = null;
                    try
                    {
                        httpReq = (HttpWebRequest)WebRequest.Create(site.url);
                        httpReq.AllowAutoRedirect = false;
                        httpRes = (HttpWebResponse)httpReq.GetResponse();
                    }
                    catch
                    {
                        update(i, 0);
                        break;
                    }

                    bool ok = false;
                    for (int a = 0; a < status_is_OK.Length; a++)
                    {
                        if ((int)httpRes.StatusCode == status_is_OK[a])
                        {
                            update(i, 2);
                            ok = true;
                            break;
                        }
                    }
                    if (!ok) update(i, 1);

                    /*
                    if (httpRes.StatusCode == HttpStatusCode.OK)
                    {
                        update(i, 2);
                    }
                    else
                    {
                        update(i, 1);
                    }*/
                    httpRes.Close();
                    break;
                }
            }
        }
    }
    class pattern_site
    {
        public string name { get; set; }
        public string url { get; set; }
        public int time { get; set; }
    }
}