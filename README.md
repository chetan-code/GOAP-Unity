# GOAP Unity AI

This repository contains essential classes for implementing Goal-Oriented Action Planning (GOAP) in Unity. GOAP is a technique used to create intelligent behaviors for NPCs in games. Below are the key classes included in this framework:

## GAction 
[view](https://github.com/chetan-code/GOAP-Unity/blob/main/Assets/Scripts/Core/GAction.cs)
- Abstract class for defining actions for game NPCs.
- Contains attributes like action name, cost, target, and duration.
- Manages preconditions and after-effects using dictionaries for efficient lookup.
- Provides methods for checking goal achievability and pre/post-action checks.

## SubGoal
[view](https://github.com/chetan-code/GOAP-Unity/blob/main/Assets/Scripts/Core/GAgent.cs)
- Class for defining sub-goals with associated conditions and removal flags.
- Used to manage multiple goals within the GOAP system.

## GAgent
[view](https://github.com/chetan-code/GOAP-Unity/blob/main/Assets/Scripts/Core/GAgent.cs)
- Manages actions, goals, and beliefs for game NPCs.
- Utilizes a planner to select and execute actions based on goals.
- Handles the execution of actions, including pre/post-action checks.
- Orchestrates decision-making and planning for NPCs.

## Node
[view](https://github.com/chetan-code/GOAP-Unity/blob/main/Assets/Scripts/Core/GPlanner.cs)
- Represents nodes in a planning graph for finding viable plans.
- Contains information about parent nodes, cost, state, and associated actions.
- Used in the planning process to construct action sequences.

## GPlanner
[view](https://github.com/chetan-code/GOAP-Unity/blob/main/Assets/Scripts/Core/GPlanner.cs)
- Plans a sequence of actions to achieve desired goals.
- Filters usable actions based on achievability.
- Constructs a planning graph to find viable plans.
- Identifies the cheapest path to achieve goals and generates action queues.

## GWorld
[view](https://github.com/chetan-code/GOAP-Unity/blob/main/Assets/Scripts/Core/GWorld.cs)
- A Singleton class for managing and accessing world states.
- Centralized repository for storing and modifying world states.
- Ensures a single instance of the world states.

## WorldState and WorldStates
- `WorldState` represents a single state in the world, consisting of a key (name) and value (magnitude).
- `WorldStates` manages a dictionary of world states and provides methods for adding, modifying, and removing states.

These classes collectively provide the foundation for implementing intelligent NPC behaviors using GOAP in Unity. Use this framework to create dynamic and goal-driven AI systems in your Unity projects.
