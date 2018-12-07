# Floomeen

Floomeen /fluːmɪ:n/ is a lightweight easy to use .Net Standard 2 class library to build finite state machines (FSMs). 

## Install

From Nuget Package Manager:

``` 
PM> Install-Package Floomeen 
``` 
or from Nuget by searching for Floomeen.

## Definitions

Let's introduce some terms before getting started, basically
States, Commands and Workflows.

### States

Any FSM has at al least a state, i.e. its particular condition at a specific time. 
For example, a light switcher tipically has an on and off state. 
Well you can immediately imagine our first switcher FSM (our first Floomeen!) with two states: On and Off.

In C# code terms Floomeen use `string struct`, and would code such `States` like:

``` 
public struct State {

     public const On = "On";

     public const Off = "Off";

}
```

### Commands

As far as the switcher goes, it need to receive some external stimulus in order to switch from one state to another. 
For example, by pushing the a button. Such altered condition is triggerd by a `Command`. 
Imagine our switcher now reacting to two commands: `SwitchOn` and `SwitchOff`. 
By code create such commads:

``` 
public struct Command {

     public const SwitchOn = "SwitchOn";

     public const SwitchOff = "SwitchOff";

}
```

Noticed that both states and commands string are, by convention, unique, case sensitive and with property name equal to its content.
Even not mandtory helps a lot while programming.

### Transitions and Workflow

A basic transition is a rule made of a begin and end state, and a command. 
A workflow establish a **set of transitions** (and constraints, see later) used by Floomeen to evolve a machine once a command is received.
Floomeen use fluent APIs to describe transitions at construction phase. 
For our switcher: `SwitchOn` would change the switcher from off to on, whilst `SwitchOff` would just do the opposite.
Floomeen code would be:

```
    Flow.AddTransition("SwitchOnTransition")
        .From(State.Off)
        .On(Command.SwitchOn)
        .GoTo(State.On);

    Flow.AddTransition("SwitchOffTransition")
        .From(State.On)
        .On(Command.SwitchOff)
        .GoTo(State.Off);

```

Easy, no?  Well... you will discover more about `Flow` /fluː/ object later on.

### Settings

In Floomeen a `Setting` is a configuration of a `State` or `Command`. Current release support state settings only.
Floomeen utilize a fluent API to describe settings. 
For example, if our switcher starts at `On` state:

```
    Flow.AddStateSetting(State.On)
        .IsStartState();
```

## The `SwitcherFloomeen`

The switcher in our example becomes `SwitcherFloomeen` class. As said following a simple workflow:

1. Start at Off state;
1. In Off state, on receiving a SwitchOn command goto On state;
1. In On state, on receiving a SwitchOff command goto Off state;

The `SwitcherFloomeen` workflow above is made of 2 transitions (2 and 3) and a state setting (1).
Clearly transitions are usually more complex, but complexity is coming soon.

Let's put all things together: 

```
    public class SwitcherFloomeen : MeenBase
    {
        public struct State
        {
            public const string On = "On";

            public const string Off = "Off";
        }

        public struct Command
        {
            public const string SwitchOn = "SwitchOn";
            
			public const string SwitchOff = "SwitchOff";
        }

        public SwitcherFloomeen ()
        {
            Flow.AddStateSetting(State.On)
              .IsStartState();

            Flow.AddTransition("SwitchOnTransition")
                .From(State.Off)
                .On(Command.SwitchOn)
					.GoTo(State.On);

            Flow.AddTransition("SwitchOffTransition")
                .From(State.On)
                .On(Command.SwitchOff)
					.GoTo(State.Off);

        }
    }
```
Notice that in Floomeen any state machine extends `MeenBase` class, where core logic lives.

Notice also the `Flow` object referenced in class constructor, is used to create the machine workflow through fluent API. 
Using an advanced code editor (like Visual Studio) you can write rules with great code-completion support (strongly recommended).
`Floow` objects smoothly allows to define settings and transitions. 
Later we will go thorough more complex transitions.

You might find helpful to put a description of rule intent passing a free string to `AddTransition` argument, 
such as `"SwitchOffTransition"` or `"Whatever this transition purpose is!"`.  

Our `SwitcherFloomeen` is born. Now what?  

Before moving on, we need to introduce Floomeen persistence mechanisms, i.e. how Floomine state s peristed over time.

### Persisting State: POCO Classes and Floomeen Attributes

To persist state, Floomeen can use any generic POCO (Plain Old CLR Object) class (see [POCO Classes](https://www.c-sharpcorner.com/UploadFile/5d065a/poco-classes-in-entity-framework/)). 
Any POCO classes can get the job done, provided it can be enriched by few Floomeen attributes. 
Let's see how it works.

## The `CustomerOrderFloomeen`

As second example, imagine an existing customers order management application implementing a `CustomerOrder` class. 
Something like:

```
public class CustomerOrder {

   public string CustomerOrderId { get; set;}

   public string CustomerId { get; set; }

   public string ShippingAddress { get; set; }

   public string Product { get; set; }

   public int Quantity { get; set; }

   public string OrderStatus { get; set; }
   ...
}
```

Naturally this is a greatly simplified data model of course.
Here `CustomerOrderId` property represents the unique key of our order, 
and `CustomerId` is a foreign key to Customer entity. A `Customer` has one-or-more `CustomerOrders`.

Clearly, our order will own a state (right?) here stored by the `OrderStatus` property. So far so good.

Imagine also our customers order going through three simple states: `New`, `Shipping` and `Delivered`.
When the order is firstly created in the system its state is `New`, waiting to get shipped. 
After cargo, while on the jorney to arrive at Customer premise, state of the order is `Shipping`. 
Finally, order state is `Delivered` right after a customer signature.

This how our `CustomerOrderFloomeen` would look like:

```

public class CustomerOrderFloomeen : MeenBase {

        public struct State
        {
            public const string New = "New";
            public const string Shipping = "Shipping";
            public const string Delivered = "Delivered";
        }

        public struct Command
        {
            public const string Cargo = "Cargo";
            public const string Hand = "Hand";
        }

        public CustomerOrderFloomeen()
        {
            Flow.AddStateSetting(State.New)
                .IsStartState();

            Flow.AddStateSetting(State.Delivered)
                .IsEndState();
            
            Flow.AddTransition("CargoTransition")
                .From(State.New)
                .On(Command.Cargo)
                .GoTo(State.Shipping);

            Flow.AddTransition("HandTransition")
                .From(State.Shipping)
                .On(Command.Hand)
                .GoTo(State.Delivered);
        }
    }

}

```

### From POCO class to Fellow

Adding two attributes to the POCO class (used to store customer order data) allow Floomeen to map its internal state:

```
public class CustomerOrder : IFellow
{

	[FloomeenId]
	public string CustomerOrderId { get; set;}

	public string CustomerId { get; set; }

	public string ShippingAddress { get; set; }

	public string Product { get; set; }

	public int Quantity { get; set; }

	[FloomeenState]
	public string OrderStatus { get; set; }

   ...
}

```

The attributes `[FloomeenId]` and `[FloomeenState]` allow Floomine to map the customer order unique identifaction and its state during the entire workflow.
In our case, both are mapped to existing properties of the POCO class.

The interface IFellow (an empty interface) allows Floomeen to manage persistency through `IFellow` interface.

Finally, the `CustomerOrderFloomeen` is ready to run, after a simple setup process:

```
	...

	var customerOrder = new CustomerOrder { 
			CustomerOrderId = "xxxx-xxxx" 
			...
	};

	var orderMeen = Factory<CustomerOrderFloomeen>.Create();

	orderMeen.Plug(customerOrder)

	...
```

The above code let Floomeen "plugging" a new customer order entity, by forcing the (empty) state of new entity to start state (`New` here). 
Notice that an Id of new customer order is required BEFORE plugging operation.
An empty or null `[FloomeenId]` attributed property (i.e. `CustomerOrderId` in our case) would raise an exception.
Object unique identification is required by master-slave Floomeen configurations, where internal events are subscribed to align master and slave workflows. 
This is an advanced topic covered later.

### Further Floomine Attributes

Floomeen can use the following further optional attributes to map useful properties:

* `[FloomineStateData]`: extends state data, even if optional, is frequently used by workflows (see later for more details) to extend state data;
* `[FloomineMachine]`: map Floomine class type name, to make sure that each entity is uniquely tight to a specific Floomine (e.g. "Mynamespace.Mymachines.CustomerOrderFloomine" );
* `[FloomineChangedOn]`: date and time of last state change (UTC);

### Delivering Our First Order

Now that our Floomine is running, we can start sending commands:

```
	...

	orderMeen.Execute(Command.Cargo);

	Console.WriteLine($"Customer Order [{customerOrder.Id}] state is '{ orderMeen.CurrentState }'");

	Console.WriteLine($"Available commands: '{string.Join(",", orderMeen.AvailableCommands() )}'");
	...
```

The above code shows a first message: `Customer Order [xxxx-xxxx] state is 'Shipping'`. 
Naturally executing any command not declared by Floomine workflow would raise an exception.
At any time, you can query the machine to obtain a list of available commands, 
in our case a second message shows: `Available commands: 'Hand'`, only `Hand` command is in fact available from state `Shipping`.

## More Example

Find more example on 


