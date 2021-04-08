using System;

namespace PublisherSubscriberTest
{
    public delegate void MyDelegate(int input);

    public class Publisher1
    {
        public MyDelegate OnChange { get; set; }
        // or you can use
        // public Action<int> _onChange;

        public void Publish(int input)
        {
            if (OnChange != null)
            {
                OnChange(input);
            }
        }
    }

    public class Subscriber1
    {
        private readonly int id;
        private readonly Publisher1 publisher;

        public Subscriber1(int id, Publisher1 publisher)
        {
            this.id = id;
            this.publisher = publisher;
            // publisher.OnChange = null; // reset
            publisher.OnChange += (input) => System.Console.WriteLine("Subcriber " + id + " received message, value " + input);
        }
    }

    public class Publisher2
    {
        public event MyDelegate OnChange;
        // or you can use
        // public event Action<int> OnChange;

        public void Publish(int input)
        {
            if (OnChange != null)
            {
                OnChange(input);
            }
        }
    }

    public class Subscriber2
    {
        private readonly int id;
        private readonly Publisher2 publisher;

        public Subscriber2(int id, Publisher2 publisher)
        {
            this.id = id;
            this.publisher = publisher;
            // publisher.OnChange = null; // Error, không thể dùng phép gán "=" ở ngoài class của event -> đảm bảo các subscriber khác không bị mất lượt sub
            publisher.OnChange += (input) => System.Console.WriteLine("Subcriber " + id + " received message");
        }
    }

    public class MyEventArgs : EventArgs
    {
        private readonly string data;

        public MyEventArgs(string data)
        {
            this.data = data;
        }

        public string GetData()
        {
            return data;
        }
    }

    public class Publisher3
    {
        public event EventHandler<MyEventArgs> OnChange;
        public void Publish()
        {
            if (OnChange != null) 
            {
                OnChange(this, new MyEventArgs("some general data"));
            }
        }
        public void WhoAmI()
        {
            System.Console.WriteLine(nameof(Publisher3));
        }
    }

    public class Subscriber3
    {
        private readonly int id;
        private readonly Publisher3 publisher;

        public Subscriber3(int id, Publisher3 publisher)
        {
            this.id = id;
            this.publisher = publisher;
            publisher.OnChange += Reciever;
        }

        private void Reciever(object sender, MyEventArgs e)
        {
            var pub = sender as Publisher3;
            pub.WhoAmI();
            System.Console.WriteLine(e.GetData());
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            // Using delegate only
           /*  var pub1 = new Publisher1();
            var sub1_1 = new Subscriber1(id: 1, publisher: pub1);
            var sub1_2 = new Subscriber1(id: 2, publisher: pub1);
            pub1.Publish(100); */

            // Thêm "event" keyword vào, cũng tương tự như trên với Publisher2 và Subscriber2, tuy nhiên phải xem phần Error

            // Khác biệt giữa delegate và event:
            // _ Event có thể được khai báo trong interface, Delegate thì không
            // _ Event chỉ có thể được gọi (invoked) ở bên trong class chứa nó, Delegate thì có thể được gọi ở bất cứ đâu (Tùy thuộc vào access modifier)
            // _ Event ngăn chặn việc gán (assign) ngoài lớp

            // Using EventHandlers
            var pub3 = new Publisher3();
            var sub3_1 = new Subscriber3(id: 1, publisher: pub3);
            var sub3_2 = new Subscriber3(id: 2, publisher: pub3);
            pub3.Publish();

            // Như vậy với EventHandler
            // object sender là publisher, establisher, noticer, broadcaster ... có chức năng truyền thông tin tới những subscriber
            // EventArgs là dữ liệu pass từ publisher đi theo            

            Console.ReadKey();
        }
    }
}
