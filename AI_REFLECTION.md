# AI Collaboration Reflection

## Overview

During this assessment, I used ChatGPT as a development assistant to help me plan, implement, test, and review my football league standings application. My intention was to use AI as a supporting software engineering tool rather than simply generating a complete solution without understanding how it worked.

I developed the application using C# and .NET 9, with xUnit for automated testing. ChatGPT helped me break down the assessment requirements, organise the project structure, and understand the responsibilities of the different components. I structured the application into models, CSV processing classes, and services to maintain a clear separation of concerns and keep the code organised, readable, and maintainable.

## Key Engineering Decisions

One of the most important decisions was ensuring that the application followed the historical football rules used during the 1974/75 English First Division season rather than modern Premier League rules.

The historical system awarded two points for a win and used goal average to rank teams that were level on points. Understanding these differences was essential because applying modern rules would produce incorrect standings. ChatGPT assisted me in understanding the historical calculations and how to implement them within the application.

Another important consideration was how the application should present its results. Although displaying the league table in the terminal was useful for reviewing the calculations, the assessment expected CSV input and output. I therefore made sure that the application could read historical match results from a CSV file and generate a separate CSV file containing the completed league standings.

I also chose to separate the standings calculation logic from file handling and create a dedicated automated testing project. This made the application easier to maintain and allowed individual components to be tested independently without depending on the entire application.

## Testing, Debugging and Validation

AI assisted me in understanding how to approach testing, debugging, and validating the application. I used xUnit to create automated tests covering football standings calculations, historical ranking rules, CSV processing, and input validation.

Throughout development, I worked incrementally rather than implementing the entire application at once. After completing each component, I built the solution, ran the relevant tests, and reviewed the results before moving forward.

I also used Git and GitHub throughout development to track changes, maintain version history, and preserve working milestones.

Rather than treating AI-generated suggestions as automatically correct, I focused on understanding their purpose and checking the results through compilation, automated testing, and reviewing the application's behaviour. This helped me identify issues during development and verify that changes were working as intended.

## Lessons Learned

This assessment helped me better understand how AI can support a structured software engineering workflow. I found it particularly useful for breaking down requirements, explaining unfamiliar concepts, exploring implementation approaches, and identifying areas that required additional testing or consideration.

The most valuable lesson was that effective AI collaboration still requires human judgement and responsibility. AI can suggest implementations, explain technical concepts, and recommend approaches, but the developer remains responsible for evaluating those suggestions, making engineering decisions, and ensuring the final solution meets the requirements.

Overall, I approached ChatGPT as a technical collaborator while maintaining responsibility for understanding, testing, and reviewing the application. The experience reinforced the importance of combining AI assistance with independent reasoning, practical testing, and sound software engineering principles.