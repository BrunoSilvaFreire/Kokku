# ECS Inventory System
This is an inventory system implementation making hybrid use of Unity's ECS, while also using traditional MonoBehaviours.

In general, the MonoBehaviour side is responsible for pooling events, and creating the appropriate "event entities", 
and ECS is responsible for the main UI logic, processing user inputs, and updating the corresponding visual 
representation.  
## Architecture
Since Unity doesnt currently provide an ECS native UI solution, and UI Toolkit doesn't have a robust animation 
integration, the elected solution is Unity's uGUI system, because it is much more consolidated and allows for
fairly simple addition of more complex animations.

However, the tradeoff is that, because uGUI is a memory managed solution, we cannot make use of Burst compiled systems, 
and must instead opt for managed system and components, which also forces us into processing the UI logic on the main 
thread. Which is acceptable given the small scope of this prototype.

Also, in order to ensure a better developer experience and avoid undefined behaviour. The Authoring components and 
initialization systems makes a couple of checks to ensure that the inventory system is not setup in an invalid state, 
and if it finds a problem, it tries it's best to guide the developer as to what is the cause.