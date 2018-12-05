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
    Floo.AddTransition("SwitchOnTransition")
        .From(State.Off)
        .On(Command.SwitchOn)
        .GoTo(State.On);

    Floo.AddTransition("SwitchOffTransition")
        .From(State.On)
        .On(Command.SwitchOff)
        .GoTo(State.Off);

```

Easy, no?  Well... you will discover more about `Floo` /fluː/ object later on.

### Settings

In Floomeen a `Setting` is a configuration of a `State` or `Command`.
Floomeen utilize a fluent API to describe settings. 
For example, if our switcher is created on an `On` as start state, we would code is as:

```
    Flow.AddSetting(State.On)
        .IsStartState();
```

# Floomeen By Examples

We use some example to better understand how Floomeen works.

## Building the `SwitcherFloomeen`

The switcher in our example not become the `SwitcherFloomeen`. 
As said following a simple workflow:

1. Start at Off state;
1. In Off state, on receiving a SwitchOn command goto On state;
1. In On state, on receiving a SwitchOff command goto Off state;

The `SwitcherFloomeen` workflow above is made of 2 transitions (2 and 3) and  
a machine setting (1).
Clearly transitions are always more complex, but relax a second because complexity is coming soon.

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
            Flow.AddSetting(State.Off)
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
Notice that in Floomeen any machine (FSM) extends the `MeenBase` class, where core logic lives.
Without exceptions our `SwitcherFloomeen` extends a `MeenBase` class. 

Notice also the `Flow` object referenced in class constructor, is used to create the machine workflow through fluent API. 
Using an advanced code editor (like Visual Studio) you can write rules with great code-completion support (strongly recommended).
The `Floo` object smoothly allows to define settings and transitions. 
Later we will go thorough more complex transitions.

You might find helpful to describe a rule by passing `AddTransition` argument, 
such as `"SwitchOffTransition"` or `"Whatever this transition does!"`.  

Our `SwitcherFloomeen` was born.
Now what?  

Well she is ready to go (forgot to tell you that a Floomeen must always be gently treated as a lady)... but wait.
Before moving on we need to provide Floomeen a persistence mechanism to store states, 
i.e. an entity to make machine states persistent over time.

# State Persitence

Bla bla bla ...

## POCO Classes and Floomeen Attributes

In order to persist state, Floomeen can use generic POCO (Plain Old CLR Object) classes (see [POCO Classes](https://www.c-sharpcorner.com/UploadFile/5d065a/poco-classes-in-entity-framework/)). 
You can use whatever POCO classes you like, provided you are able to enrich properties with Floomeen attributes.  
Let's see how it works.

## `CustomerOrder` POCO Class Example

Imaging our customers order management application using some a `CustomerOrder` class, something like:

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
`CustomerOrderId` property represents a unique key of our order, 
and `CustomerId` is a foreign key to Customer entity. 

Clearly, our CustomerOrder owns a state (right?) on our case the `OrderStatus` property. 
So far so good.

Now imaging to use Floomeen for managing a real customer order FSM workflow. 
We definitively to persist state data using `CustomerOrder` POCO entity.

Before processing let's take a step back.  
Suppose Customer order to go through three simple states: `New`, `Shipping` and `Delivered`.
When the order is firstly created in the system its state is `New`, waiting to get shipped. 
While on the jorney to arrive at Customer location once departed from our warehouse, the state of the customer order will be `Shipping`. 
Finally the order status will change to `Delivered` right after customer signature.

This how our `CustomerOrderFloomeen` would look like:

```
public class CustomerOrderFloomeen : MineBase {

        public struct State
        {
            public const string New = "New";
            public const string Shipping = "Shipping";
            public const string Delivered = "Shipping";
        }

        public struct Command
        {
            public const string Cargo = "Cargo";
            public const string Hand = "Hand";
        }

        public CustomerOrderFloomeen()
        {
            Floo.AddSetting(State.New)
                .IsStartState();

            Floo.AddSetting(State.Delivered)
                .IsEndState();
            
            Floo.AddTransition("CargoTransition")
                .From(State.New)
                .On(Command.Cargo)
                .GoTo(State.Shipping);

            Floo.AddTransition("HandTransition")
                .From(State.Shipping)
                .On(Command.Hand)
                .GoTo(State.Delivered);
        }
    }

}
```

## From POCO class to Fellow


