using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;

namespace TShirt.InventoryApp.Integration
{
    public class ObserverManager
    {
        // SINGLETON //
        private static volatile ObserverManager instance;
        private static object syncRoot = new Object();

        private ObserverManager() {
            msSubject = new Queue<ISubject>();
            msObserver = new List<IObserver>();
        }
        public static ObserverManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ObserverManager();
                    }
                }

                return instance;
            }
        }
         // SINGLETON //

        private Queue<ISubject> msSubject;
        private List<IObserver> msObserver;
        

        public void addSubject(ISubject sub)
        {
            msSubject.Enqueue(sub);
        }

        public void addObserver(IObserver obs)
        {
            msObserver.Add(obs);
        }

        public void publish()
        {
            ISubject s;
            try
            {

                while((s=msSubject.Dequeue())!= null)
                {
                    foreach (IObserver ob in msObserver)
                    {
                        ob.Publish(s.toString());
                    }
                }
            }catch(Exception ex)
            {
                //MessageBox.Show("Observer Manager::"+ex.Message);

            }           
        }


    }
}
