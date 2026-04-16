# HUNGER

## Game Can Be Played Here (Access Via Browser): https://kenzieherrick.itch.io/hunger 

## Project Overview
Hunger is a 3D first-person narrative survival game developed in Unity. The player must survive 10 winter days (December 1 to 10) while managing systemic sacrifices across
three hidden values: Family, Home, and Self. The experience focuses on environmental storytelling, psychological pressure, and cyclical survival.

The player lives on a small farm with their family and a starving creature named Don, who makes vague requests every morning. Each day, the player must choose an object to give up in order
to feed Don, unknowingly sacrificing aspects of their home, family, or self. These systems degrade overtime and are reflected through environmental changes and character behavior.

The core experience centers on player choice without a clear path, encouraging uncomfortable trade-offs rather than "correct" choices. Endings are determined by which system
collapses first, or by surviving until December 10th with the lasting consequences.

## Mechanics
- **Daily Survival Loop:**  
  Each day consists of exploration, character interaction, receiving a request from Don, and selecting an item to sacrifice.

- **Sacrifice System:**  
  Players choose from discovered items to give up at the end of each day. Each item belongs to one of three categories: Home, Family, or Self, and sacrificing them reduces that category.

- **Category System:**  
  The game tracks three core values (Home, Family, and Self) which represent the player’s stability. These values decrease over time based on player decisions and events.

- **Dynamic Events:**  
  Daily events introduce unpredictability by removing items, providing bonuses, or creating additional pressure on the player’s decisions.

- **Multiple Endings:**  
  The outcome of the game is determined by which category is most depleted by the end of the 10 days, or if any category reaches zero early.
  
## Controls
- **Mouse** – Select items and make choices
- **WASD** – Move Outside of House  
- **ESC** – Pause menu  

## Special Features

- **Visual Stat Tracking:**  
  Home, Family, and Self are represented through **icon-based stat bars** visible in the UI. These provide continuous feedback on the player’s condition and are introduced through the in-game instructions.

- **Narrative-Driven Gameplay:**  
  Dialogue, environmental changes, and events evolve based on player decisions, reinforcing the psychological impact of sacrifice.

- **Randomized Item Availability:**  
  Each day, available items and event outcomes vary, ensuring no two playthroughs are exactly the same.

- **Thematic UI and Presentation:**  
  Visual elements such as hand-drawn intro sequences and minimal UI design reinforce the perspective of a young boy and the unsettling tone of the experience.


## Development Log

**Apr 15, 2026**
- Finalized UI implementation and polished player-facing systems
- Balanced core gameplay loop, including sacrifice outcomes and stat impact
- Cleaned and organized project structure for final submission

**Apr 12, 2026**
- Refined major gameplay systems for clarity and consistency
- Simplified mechanics to improve player understanding and reduce confusion
- Adjusted event structure to better support gameplay tension

**Apr 6, 2026**
- Implemented sacrifice item system and integrated it into gameplay loop
- Established item categorization (Home, Family, Self)
- Began connecting item selection to outcome-based consequences

**Apr 5, 2026**
- Expanded narrative systems including dialogue, daily events, and request structure
- Developed branching dialogue flow for morning interactions
- Strengthened narrative integration with gameplay mechanics

**Mar 28, 2026**
- Built full gameplay loop including exploration, dialogue, and sacrifice phases
- Integrated dialogue system across multiple game states
- Connected UI and gameplay systems for player progression

**Mar 20, 2026**
- Added environmental assets to populate and support game world
- Expanded bathroom interaction system as part of exploration design
- Improved scene detail and player interaction points

**Mar 15, 2026**
- Developed navigation system between rooms
- Implemented item interaction mechanics
- Established core exploration framework

**Mar 4, 2026**
- Imported primary environment assets including farmhouse and barn
- Set up terrain and exterior gameplay space
- Organized scene layout for interior and exterior transitions

**Feb 21, 2026**
- Blocked out game environment and added Unity Asset First-Person Character Controller
- Completed MVP development and documentation
- Incorporated MVP feedback from initial playtesting into prototype direction

**Feb 14, 2026**
- Created GitHub repository and Unity project
- Established organized folder hierarchy
- Implemented namespace structure
- Added initial placeholder scripts

## Milestones & Progress
**Completed**
 - Project setup and core planning
 - GitHub repo created and set to public
 - Unity project initialized
 - Folder hierarchy reorganized
 - Namespaces set for core systems
 - Placeholder scripts created for: GameManager, StatSystem, DialougeManager, and SacrificeSystem
 - Initial README documentation added
 - Implement daily gameplay loop structure
 - Begin stat tracking
 - Basic player movement and interaction
 - Initial dialogue triggers
 - Fully functioning sacrifice system
 - Environmental feedback based on stat thresholds
 - Day progression (Dec 1 to Dec 10)
 - Narrative dialogue integration
 - Playtesting and balancing
 - Bug report and triage (Due April 5)
 - Visual and audio polish
 - Final build and documentation (Due April 19)

## MVP Development
For the MVP of Hunger, I focused on implementing the core gameplay loop and foundational systems that define the game’s central experience: sacrifice as survival.
The MVP consists of a playable three-day winter cycle that demonstrates a daily loop. Each day begins with an abstract request generated by the creature, Don (e.g., “Warmth” or “Comfort”). The player can explore a small 3D farmhouse environment and discover an array of interactable items. Once discovered, these items become available for sacrifice.
Each item is implicitly tied to one of three hidden categories: Home, Self, or Family. When the player sacrifices an item, the corresponding category value decreases. These values are not shown directly to the player but function as underlying survival systems through world feedback (e.g., lighting changes). If any category reaches zero, the player loses. If the player survives all three days, they win the prototype scenario.

The MVP includes:
- A functional day cycle system
- Randomized request generation
- Item discovery and sacrifice mechanics
- A stat reduction and fail-state system
- Basic win/lose conditions
- Core player controls and interaction logic
  
Rather than building full narrative content or multiple months of gameplay, the MVP prioritizes validating the central mechanic: making meaningful sacrifices under discovered resources. The prototype establishes a scalable system that can be expanded with more in-depth environmental feedback, narrative consequences, and additional content in future development phases. 

MVP playthroughs can be seen on the Trello Board linked in "Resources"

## MVP Feedback
During early testing of the MVP, several areas for improvement were identified.
One common piece of feedback was that the day cycle didn't feel structured. Testers were unsure when a day ended and when a new one began. In response, I refined the day progression system to automatically advance after a sacrifice and ensured that each new day generates a clear request at the start. This helped reinforce the intended gameplay loop.
Another point of feedback was that the stakes of sacrificing items were not immediately obvious. Because the Home, Self, and Family categories are hidden systems, some testers did not initially recognize how their choices affected survival. To address this, I clarified the consequences through stronger feedback via lighting changes tied to the stat system (as a full narrative tree/textured models aren't part of the MVP), reinforcing the cause-and-effect relationship between sacrifice and survival. Essentially, the categories affected the brightness/dimness and warmth of the scene. If the player sacrificed items in a more balanced way then the lighting remained natural. However, if their sacrifices skewed to one category, they saw the impacts on the world around them.
Additionally, early testers attempted to sacrifice items that had not been discovered or expected more item variety per day. Based on this feedback, I gave the players numerous available items they could discover and based on the request they chose which item they felt most "drawn" to. This simplified the system and strengthened the tension of choice, better highlighting the game’s central theme.
These iterations improved clarity, strengthened the core loop, and ensured the MVP better communicates the intended emotional and mechanical experience.


## Challenges & Solutions

**1. Overly Complex Gameplay Systems**
- **Challenge:**  
Early in development, the game included multiple systems such as item values, sacrifice weights, and stat modifiers. While these systems worked individually, together they made the game confusing and difficult for players to understand during playtesting.
- **Solution:**  
The systems were simplified by standardizing item values and removing unnecessary calculations like sacrifice weight. This made the core mechanic—choosing what to sacrifice—much clearer while still maintaining meaningful decision-making.

**2. Lack of Player Clarity**
- **Challenge:**  
Playtesters struggled to understand what actions were available at different points in the day, especially during the morning and exploration phases.
- **Solution:**  
Visual guidance replaced text-based restrictions. Instead of blocking actions with dialogue, UI buttons were disabled and faded to indicate unavailable options. Additionally, a short introductory sequence was added to establish the game’s rules and flow in a more intuitive way.

**3. Low Gameplay Tension**
- **Challenge:**  
Players were consistently able to survive all 10 days because the game did not apply enough pressure. Items were always available, and consequences were not impactful enough.
- **Solution:**  
The event system was redesigned so that many negative events remove random items from the available pool. This reduced player certainty and increased tension, forcing more difficult decisions. Sacrifice penalties were also increased to make mismatches more impactful.

**4. Repetitive or Unclear Event Design**
- **Challenge:**  
Initial event designs were vague and did not clearly communicate their impact on gameplay, making them feel disconnected from player decisions.
- **Solution:**  
Events were simplified and made more direct. Instead of abstract effects, events now clearly state outcomes (such as item loss or bonuses), making their impact immediately understandable and meaningful.

## Resources & References
- Unity (3D Core)
- GitHub for version control
- Trello for project planning: https://trello.com/b/o0SbBJPp/capstone-project
- Unity Asset Store First-Person Character Controller
- Pixabay snow walking audio thanks to "spinopel"
- MVP code and final game code developed with the help of GitHub Copilot and Unity Documentation

## Team Contributions
Kenzie Herrick - Solo Developer - Design, Programming, Narrative, Systems Implementation

## Final Notes
The project evolved significantly from its initial concept, shifting toward a more streamlined and readable system focused on player decision-making and sacrifice. Iteration focused mainly on reducing confusion while maintaining narrative tension.
