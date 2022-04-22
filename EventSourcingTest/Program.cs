#nullable disable

namespace ConsoleApp1
{
    //public record Person(
    //    string Name,
    //    string Address
    //);

    //public record InvoiceInitiated(
    //    double Amount,
    //    string Number,
    //    Person IssuedTo,
    //    DateTime InitiatedAt
    //);

    //public record InvoiceIssued(
    //    string IssuedBy,
    //    DateTime IssuedAt
    //);

    //public enum InvoiceSendMethod
    //{
    //    Email,
    //    Post
    //}

    //public record InvoiceSent(
    //    InvoiceSendMethod SentVia,
    //    DateTime SentAt
    //);

    //public enum InvoiceStatus
    //{
    //    Initiated = 1,
    //    Issued = 2,
    //    Sent = 3
    //}

    //public class Invoice
    //{
    //    public string Id { get; set; }
    //    public double Amount { get; private set; }
    //    public string Number { get; private set; }

    //    public InvoiceStatus Status { get; private set; }

    //    public Person IssuedTo { get; private set; }
    //    public DateTime InitiatedAt { get; private set; }

    //    public string IssuedBy { get; private set; }
    //    public DateTime IssuedAt { get; private set; }

    //    public InvoiceSendMethod SentVia { get; private set; }
    //    public DateTime SentAt { get; private set; }

    //    public void When(object @event)
    //    {
    //        switch (@event)
    //        {
    //            case InvoiceInitiated invoiceInitiated:
    //                Apply(invoiceInitiated);
    //                break;
    //            case InvoiceIssued invoiceIssued:
    //                Apply(invoiceIssued);
    //                break;
    //            case InvoiceSent invoiceSent:
    //                Apply(invoiceSent);
    //                break;
    //        }
    //    }

    //    private void Apply(InvoiceInitiated @event)
    //    {
    //        Id = @event.Number;
    //        Amount = @event.Amount;
    //        Number = @event.Number;
    //        IssuedTo = @event.IssuedTo;
    //        InitiatedAt = @event.InitiatedAt;
    //        Status = InvoiceStatus.Initiated;
    //    }

    //    private void Apply(InvoiceIssued @event)
    //    {
    //        IssuedBy = @event.IssuedBy;
    //        IssuedAt = @event.IssuedAt;
    //        Status = InvoiceStatus.Issued;
    //    }

    //    private void Apply(InvoiceSent @event)
    //    {
    //        SentVia = @event.SentVia;
    //        SentAt = @event.SentAt;
    //        Status = InvoiceStatus.Sent;
    //    }
    //}

    public class Person
    {
        private readonly EventBroker _broker;

        private string _name;
        private int _age = -1;
        
        public Person(EventBroker broker)
        {
            _broker = broker;
            _broker.Commands += BrokerOnCommand;
            _broker.Queries += BrokerOnQuery;
        }

        private void BrokerOnQuery(object sender, Query query)
        {
            var ac = query as AgeQuery;
            if (ac != null && ac.Target == this)
            {
                ac.Result = _age;
            }
        }

        private void BrokerOnCommand(object sender, Command command)
        {
            var cac = command as ChangeAgeCommand;
            if (cac != null && cac.Target == this)
            {
                if (cac.Register)
                {
                    _broker.AllEvents.Add(new AgeChangedEvent(this, _age, cac.Age));
                }
                _age = cac.Age;
            }
        }
    }

    public class Event
    {
        // Backtrack
    }

    public class AgeChangedEvent : Event
    {
        public Person Target { get; set; }
        public int OldValue { get; set; }
        public int NewValue { get; set; }

        public AgeChangedEvent(Person target, int oldValue, int newValue)
        {
            Target = target;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public override string ToString()
        {
            return $"Age changed from {OldValue} to {NewValue}";
        }
    }

    public class EventBroker
    {
        public IList<Event> AllEvents { get; set; } = new List<Event>();
        public event EventHandler<Command> Commands;
        public event EventHandler<Query> Queries;

        public void Command(Command command)
        {
            Commands.Invoke(this, command);
        }

        public T Query<T>(Query query)
        {
            Queries?.Invoke(this, query);
            return (T) query.Result;
        }

        public void UndoLast()
        {
            var e = AllEvents.LastOrDefault();
            var ac = e as AgeChangedEvent;
            if (ac != null)
            {
                Command(new ChangeAgeCommand(ac.Target, ac.OldValue) { Register = false });
                AllEvents.Remove(e);
            }
        }
    }

    #region Queries and Commands
    
    public class Query
    {
        public object Result { get; set; }
    }

    public class Command : EventArgs
    {
        public bool Register { get; set; } = true;
    }

    public class AgeQuery : Query
    {
        public Person Target { get; set; }
    }

    public class ChangeAgeCommand : Command
    {
        public Person Target { get; private set; }
        public int Age { get; private set; }

        public ChangeAgeCommand(Person target, int age)
        {
            Age = age;
            Target = target;
        }
    }

    #endregion

    public class Program
    {
        public static void Main(string[] args)
        {
            var eb = new EventBroker();
            var p = new Person(eb);
            
            // Command
            eb.Command(new ChangeAgeCommand(p, 26));
            foreach (var e in eb.AllEvents)
            {
                Console.WriteLine(e);
            }

            // Query
            int age;
            age = eb.Query<int>(new AgeQuery() { Target = p });
            Console.WriteLine(age);

            // Undo/Revert -> important function
            eb.UndoLast();
            foreach (var e in eb.AllEvents)
            {
                Console.WriteLine(e);
            }

            // Query 2
            age = eb.Query<int>(new AgeQuery() { Target = p });
            Console.WriteLine(age);

            //var invoiceInitiated = new InvoiceInitiated(
            //    34.12,
            //    "INV/2021/11/01",
            //    new Person("Oscar the Grouch", "123 Sesame Street"),
            //    DateTime.UtcNow
            //);

            //var invoiceIssued = new InvoiceIssued(
            //    "Cookie Monster",
            //    DateTime.UtcNow
            //);

            //var invoiceSent = new InvoiceSent(
            //    InvoiceSendMethod.Email,
            //    DateTime.UtcNow
            //);

            //// 1,2. Get all events and sort them in the order of appearance
            //var events = new object[] { invoiceInitiated, invoiceIssued, invoiceSent };

            //// 3. Construct empty Invoice object
            //var invoice = new Invoice();

            //// 4. Apply each event on the entity.
            //foreach (var @event in events)
            //{
            //    invoice.When(@event);
            //}
        }
    }

}

//public class A
//{
//    public int MyProperty { get; set; }

//    public void Run()
//    {
//        X(this);
//    }

//    public void X(object sender)
//    {
//        var x = sender as A;
//        Console.WriteLine(x.MyProperty);
//    }
//}


