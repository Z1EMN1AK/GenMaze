
# Genetic Algorithm for Pathfinding

This project implements a genetic algorithm to solve a pathfinding problem. The program aims to find an optimal path from a starting position to an endpoint on a given map. It demonstrates the use of genetic algorithms to evolve solutions over generations.

## Features

- **Dynamic Pathfinding:** The program adapts and evolves paths based on fitness evaluations.
- **Genetic Algorithm Techniques:** Includes mutation, crossover, and fitness evaluation.
- **Customizable Options:** Configure parameters such as map path, logging, and generation intervals.
- **Console Visualization:** Displays generation progress and solution paths in the console.

## Project Structure

### Classes

- `Position`

  - Represents a coordinate on the map with `x` and `y` properties.

- `Population`

  - Represents a group of specimens. Manages sorting, evaluation, and interaction with the map.

- `Specimen`

  - Encodes a potential solution as a sequence of genetic instructions (`L`, `R`, `U`, `D`). Includes methods for mutation, crossover, and fitness evaluation.

- `Map`

  - Loads and represents the map used for pathfinding. Reads map boundaries and obstacles from a file.

- `AlgoObject`

  - The core class that orchestrates the genetic algorithm. Manages generations, mutation rates, and selection processes.

- `Options`

  - Struct used to configure the behavior of the program, including generation intervals, logging preferences, and file paths.

- `Program`

  - Entry point of the application. Handles user input for configuration and starts the algorithm.

 - `MazeGenerator`
   - The LabGen class in MazeGenerator.cs is used to generate a maze using a depth-first search (DFS) algorithm.

## How It Works

1. **Initialization:**

   - Load the map from a file.
   - Define starting and ending positions.
   - Generate an initial population of specimens.

2. **Fitness Evaluation:**

   - Evaluate each specimen's fitness based on distance to the endpoint, collisions, and traversal efficiency.

3. **Selection:**

   - Sort specimens by fitness.
   - Select a subset for crossover.

4. **Crossover and Mutation:**

   - Perform heuristic crossover between specimens.
   - Apply random mutations to introduce variability.

5. **Generation Progress:**

   - Repeat the process for a specified number of generations or until a solution is found.

6. **Logging and Visualization:**

   - Log results to a file (optional).
   - Display intermediate and final results in the console.

## Setup and Usage

1. **Prerequisites:**

   - .NET Framework 6.0 or higher.
   - Visual Studio or any C#-compatible IDE.

2. **Running the Program:**

   - Clone the repository.
   - Open the solution in your IDE.
   - Build and run the project.

3. **Configuration:**

   - Provide the map file path when prompted.
   - Configure logging preferences and generation intervals via the console prompts.

4. **Map File Format:**

   - The map file should contain:
     - `S` for the start position.
     - `E` for the end position.
     - `#` for obstacles.
     - `.` for walkable paths.

## Example

Here is an example map file:

```
#################S#####################
#               #         #           #
# ###### ######## ####### # ###########
#      #          #     # # #         #
# ### ########### ####### # # ######  #
# #               #     # # #      #  #
# #      ######## # ### # # ########  #
# ########      # # # #               #
#        ######## #   # ###############
# ######          ##### #    #        #
# ###### ######## #     #    # ###### #
#        #      # ############ ##     #
# ###### #      #               # #####
# #    # ############### ###### # #   #
# #    #                        # #   #
# #    # ######################## #####
# #    #                              #
# ###### ############ ######### # #####
#        #                    # # #   #
###############################E#######
```

## Customization

- **Map Size:** Adjust the dimensions and layout in the map file.
- **Population Size:** Modify the initial population size in `AlgoObject`.
- **Mutation Rate:** Change the mutation rate logic in `AlgoObject`.

## Changelog
### Update 08.01.2025
- Added MazeGenerator.cs with DFS algorithm for maze generation.
- Improved constructors for better initialization.
- Added dynamic mutation rate adjustment and map generation logic.
- Updated fitness evaluation logic.
- General code readability and efficiency improvements.

## Future Enhancements

- Add a graphical interface for better visualization.
- Support for different types of maps, such as weighted grids.
- Incorporate advanced genetic operators.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request with your improvements.

## License

This project is licensed under the MIT License.
