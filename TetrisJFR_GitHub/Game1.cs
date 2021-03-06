﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System;
using System.Collections.Generic;

namespace TetrisJFR_GitHub
{
    /*
        The Game1.cs file contains mostly functions that we have created that are not
        a part of the MonoGame Library. This file is also the main type for our game.

        It also contains the variables that we have used for our program. Such as the
        arrays that we have used to store vital game data, such as where blocks are 
        placed, along with their corresponding coordinates, and what blocks they 
        represent

        List of functions include:
        - KeyboardHandler()
        - randomNumberGenerator()      
    */

    public partial class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Texture for the playing grid
        Texture2D blockForGrid;

        // Object for the falling block that spawns
        fallingBlock currentBlock;

        // Object used to clear rows, by creating blocks that come from the playing grid
        // By doing so, it gives the illusion that the row has been cleared
        fallingBlock deleteBlock;

        // Object used to bring a block down, if the row has been cleared
        fallingBlock bringDownBlock;

        // Texture for the Tetris Logo
        Texture2D tetrisLogo;

        // Creating text for the score
        SpriteFont scoreText;

        // Used when handling Keyboard Inputs
        private KeyboardState oldState;

        //Pause flag initialization:
        int pauseFlag = 0;

        //background for the menu
        Texture2D startGameSplash;

        //check if game started
        bool gameStarted;

        // Initializing our timer
        // Source used to create timer:
        // https://stackoverflow.com/questions/13394892/how-to-create-a-timer-counter-in-c-sharp-xna
        int counter = 0;
        float countDuration = 1; // one second
        float currentTime = 0;

        int score = 0;

        /*
            When deleting blocks or clearing rows, have an array that has all the characteritics
            of the block, so you can bring it down later using the draw function
         
        */

        int xBoard;
        int yBoard;

        // Multiple variable instances used to track the coordinates of different objects
        // on the board, since they all have different sizes.  
        int xBoard2;
        int xBoard3;
        int xBoard4;

        int yBoard2;
        int yBoard3;
        int yBoard4;

        int blockType = 3; // initialize the first object to spawn for testing purposes

        // 20 rows, 10 columns
        int[,] digitalBoard = new int[20, 10]
        {                  //x <-- start here
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };

        int[,] blockColorArray = new int[20, 10]
        {
            // -21 represends a grey "grid" block
                                     //x <-- start here
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //1
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //2
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //3
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //4
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //5
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //6
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //7
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //8
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //9
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 },//10
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //11
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //12
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 },//13
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //14
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //15
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //16
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //17
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //18
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 }, //19
                {-21, -21, -21, -21, -21, -21, -21, -21, -21, -21 },  //20th row
        };

        int[,] locationYArray = new int[20, 10]
        {                  //x <-- start here
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {20, 20, 20, 20, 20, 20, 20, 20, 20, 20 },
                {40, 40, 40, 40, 40, 40, 40, 40, 40, 40 },
                {60, 60, 60, 60, 60, 60, 60, 60, 60, 60 },
                {80, 80, 80, 80, 80, 80, 80, 80, 80, 80 },
                {100, 100, 100, 100, 100, 100, 100, 100, 100, 100 },
                {120, 120, 120, 120, 120, 120, 120, 120, 120, 120 },
                {140, 140, 140, 140, 140, 140, 140, 140, 140, 140 },
                {160, 160, 160, 160, 160, 160, 160, 160, 160, 160 },
                {180, 180, 180, 180, 180, 180, 180, 180, 180, 180 },
                {200, 200, 200, 200, 200, 200, 200, 200, 200, 200 },
                {220, 220, 220, 220, 220, 220, 220, 220, 220, 220 },
                {240, 240, 240, 240, 240, 240, 240, 240, 240, 240 },
                {260, 260, 260, 260, 260, 260, 260, 260, 260, 260 },
                {280, 280, 280, 280, 280, 280, 280, 280, 280, 280 },
                {300, 300, 300, 300, 300, 300, 300, 300, 300, 300 },
                {320, 320, 320, 320, 320, 320, 320, 320, 320, 320 },
                {340, 340, 340, 340, 340, 340, 340, 340, 340, 340 },
                {360, 360, 360, 360, 360, 360, 360, 360, 360, 360 },
                {380, 380, 380, 380, 380, 380, 380, 380, 380, 380 }
        };

        int[,] locationXArray = new int[20, 10]
        {                  //x <-- start here
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 },
                {0, 20, 40, 60, 80, 100, 120, 140, 160, 180 }
        };

        List<int> highscores = new List<int>() { 0, 0, 0 };
        bool scoreAlreadyAdded = false;

        int generateNewObject = 0;

        bool gameOver = false;
        bool clearTheBoard = false;

        // Default Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /*
        // The KeyboardHandler() handles all the user keyboard inputs.

            In this case, we have programmed the "A" and "D" keys for the 
            user to move the block left and right

            - We have used the "P" key to allow the player to lower the block
            by one row. Pressing this button frequently will allow the
            user to bring the block down faster.
            - "Enter" is used to start the game
            - "ESC" is used to exit the game
            - "M" is used to pause the game.
            - "R" is used to reset the game, once the game ends

            This KeyboardHandler() also contains code that forces
            the user's block to stay within the playing area. Should they try
            exit the range of the playing field, the code wont allow them to pass.

        */
        void KeyboardHandler()
        {

            KeyboardState newState = Keyboard.GetState();

            //The menu will show if the player does not press the ENTER key
            //but the game will start if the ENTER key is pressed
            if (!gameStarted)
            {
                if (oldState.IsKeyUp(Keys.Enter) && newState.IsKeyDown(Keys.Enter) && !gameOver)
                {
                    score = 0;
                    // r changes

                    int numRow = 20;
                    int l;
                    int p;
                    int numCol = 10;
                    for (l = 0; l < numRow; ++l)
                    {
                        for (p = 0; p < numCol; ++p)
                        {
                            digitalBoard[l, p] = 0;
                            blockColorArray[l, p] = -21;
                        }
                    }

                    clearTheBoard = true;

                    // end of r changes
                    scoreAlreadyAdded = false;
                    gameOver = false;

                    gameStarted = true;
                }
                return;
            }
                
                //press esc to exit the game
                if (oldState.IsKeyUp(Keys.Escape) && newState.IsKeyDown(Keys.Escape) && !gameOver)
                {
                    Exit();
                }

                //press the M to pause the game
                if (oldState.IsKeyUp(Keys.M) && newState.IsKeyDown(Keys.M) && !gameOver)
                {
                    pauseFlag ^= 1;
                }

                //Pressing R to restart: resets score and the board
                if (oldState.IsKeyUp(Keys.R) && newState.IsKeyDown(Keys.R) && gameOver)
                {
                    score = 0;
                    // r changes

                    int numRow = 20;
                    int l;
                    int p;
                    int numCol = 10;
                    for (l = 0; l < numRow; ++l)
                    {
                        for (p = 0; p < numCol; ++p)
                        {
                            digitalBoard[l, p] = 0;
                            blockColorArray[l, p] = -21;
                        }
                    }

                    clearTheBoard = true;

                    // end of r changes
                    scoreAlreadyAdded = false;
                    gameOver = false;


                }

                // Move Left
                if (oldState.IsKeyUp(Keys.A) && newState.IsKeyDown(Keys.A) && !gameOver && pauseFlag == 0)
                {
                    if (blockType == 0)
                    {
                        if (currentBlock.x - 20 <= -1
                            || digitalBoard[yBoard, xBoard - 1] == 1)
                        {
                            // Dont move the object 
                        }
                        else
                        {
                            currentBlock.x -= 20;
                            xBoard -= 1;
                        }
                    }
                    else if (blockType == 1)
                    {
                        if (currentBlock.x - 20 <= -1
                            || digitalBoard[yBoard, xBoard - 1] == 1)
                        {
                            // Dont move the object 
                        }
                        else
                        {
                            currentBlock.x -= 20;
                            xBoard -= 1;
                            xBoard2 -= 1;
                            xBoard3 -= 1;
                        }
                    }
                    else if (blockType == 2)
                    {
                        if (currentBlock.x - 20 <= -1
                            || digitalBoard[yBoard, xBoard - 1] == 1
                            || digitalBoard[yBoard2, xBoard - 1] == 1
                            || digitalBoard[yBoard3, xBoard - 1] == 1)
                        {
                            // Dont move the object
                        }

                        else
                        {
                            currentBlock.x -= 20;
                            xBoard -= 1;
                            xBoard2 -= 1;
                            xBoard3 -= 1;
                        }

                    }
                    else if (blockType == 3)
                    {
                        if (currentBlock.x - 20 <= -1
                            || digitalBoard[yBoard, xBoard - 1] == 1
                            || digitalBoard[yBoard2, xBoard2 - 1] == 1
                            || digitalBoard[yBoard3, xBoard3 - 1] == 1)
                        {
                            // Dont move the object 
                        }
                        else
                        {
                            currentBlock.x -= 20;
                            xBoard -= 1;
                            xBoard2 -= 1;
                            xBoard3 -= 1;
                            xBoard4 -= 1;
                        }
                    }
                    else if (blockType == 4)
                    {
                        if (currentBlock.x - 20 <= -1
                            || digitalBoard[yBoard, xBoard] == 1
                            || digitalBoard[yBoard2, xBoard2] == 1
                            || digitalBoard[yBoard4, xBoard4] == 1)
                        {
                            // Dont move the object 

                        }
                        else
                        {
                            currentBlock.x -= 20;
                            xBoard -= 1;
                            xBoard2 -= 1;
                            xBoard3 -= 1;
                            xBoard4 -= 1;
                        }
                    }
                    else if (blockType == 5)
                    {
                        if (currentBlock.x - 20 <= -1
                            || digitalBoard[yBoard, xBoard - 1] == 1
                            || digitalBoard[yBoard3, xBoard3 - 1] == 1)
                        { }
                        else
                        {
                            currentBlock.x -= 20;
                            xBoard -= 1;
                            xBoard2 -= 1;
                            xBoard3 -= 1;
                            xBoard4 -= 1;
                        }
                    }
                    else if (blockType == 6)
                    {
                        if (currentBlock.x - 20 <= -1
                            || digitalBoard[yBoard, xBoard - 1] == 1
                            || digitalBoard[yBoard3, xBoard3 - 1] == 1)
                        { }
                        else
                        {
                            currentBlock.x -= 20;
                            xBoard -= 1;
                            xBoard2 -= 1;
                            xBoard3 -= 1;
                            xBoard4 -= 1;
                        }
                    }
                    else if (blockType == 7)
                    {
                        if (currentBlock.x - 20 <= -1
                            || digitalBoard[yBoard, xBoard - 1] == 1
                            || digitalBoard[yBoard3, xBoard3 - 1] == 1)
                        { }
                        else
                        {
                            currentBlock.x -= 20;
                            xBoard -= 1;
                            xBoard2 -= 1;
                            xBoard3 -= 1;
                            xBoard4 -= 1;
                        }
                    }
                    else if (blockType == 8)
                    {
                        if (currentBlock.x - 20 <= -1
                            || digitalBoard[yBoard, xBoard - 1] == 1
                            || digitalBoard[yBoard2, xBoard2 - 1] == 1
                            || digitalBoard[yBoard4, xBoard4 - 1] == 1)
                        { }
                        else
                        {
                            currentBlock.x -= 20;
                            xBoard -= 1;
                            xBoard2 -= 1;
                            xBoard3 -= 1;
                            xBoard4 -= 1;
                        }
                    }
                    else if (blockType == 9)
                    {
                        if (currentBlock.x - 20 <= -1
                            || digitalBoard[yBoard, xBoard - 1] == 1
                            || digitalBoard[yBoard3, xBoard3 - 1] == 1)
                        { }
                        else
                        {
                            currentBlock.x -= 20;
                            xBoard -= 1;
                            xBoard2 -= 1;
                            xBoard3 -= 1;
                            xBoard4 -= 1;
                        }
                    }
                }


                // Move Right
                else if (oldState.IsKeyUp(Keys.D) && newState.IsKeyDown(Keys.D) && !gameOver && pauseFlag == 0)
                {
                    if (blockType == 0)
                    {
                        if (xBoard + 1 >= 10
                            || digitalBoard[yBoard, xBoard + 1] == 1)
                        {
                            // Dont move the object 
                        }

                        else
                        {
                            currentBlock.x += 20;
                            xBoard += 1;
                        }
                    }
                    else if (blockType == 1)
                    {
                        if (xBoard + 3 >= 10
                            || digitalBoard[yBoard, xBoard3 + 1] == 1)
                        {
                            // Dont move the object 
                        }
                        else
                        {
                            currentBlock.x += 20;
                            xBoard += 1;
                            xBoard2 += 1;
                            xBoard3 += 1;
                        }
                    }
                    else if (blockType == 2)
                    {
                        if (xBoard + 1 >= 10 || xBoard2 + 1 >= 10 || xBoard3 + 1 >= 10
                            || digitalBoard[yBoard, xBoard + 1] == 1
                            || digitalBoard[yBoard2, xBoard + 1] == 1
                            || digitalBoard[yBoard3, xBoard + 1] == 1)
                        {
                            // Dont move the object 
                        }
                        else
                        {
                            currentBlock.x += 20;
                            xBoard += 1;
                            xBoard2 += 1;
                            xBoard3 += 1;
                        }

                    }
                    else if (blockType == 3)
                    {
                        if (xBoard + 2 >= 10
                            || digitalBoard[yBoard, xBoard + 1] == 1
                            || digitalBoard[yBoard2, xBoard2 + 1] == 1
                            || digitalBoard[yBoard4, yBoard4 + 1] == 1)
                        {
                            // Dont move the object 
                        }
                        else
                        {
                            currentBlock.x += 20;
                            xBoard += 1;
                            xBoard2 += 1;
                            xBoard3 += 1;
                            xBoard4 += 1;
                        }
                    }
                    else if (blockType == 4)
                    {
                        if (xBoard + 2 >= 10
                            || digitalBoard[yBoard, xBoard + 1] == 1
                            || digitalBoard[yBoard3, xBoard3 + 1] == 1
                            || digitalBoard[yBoard4, xBoard4 + 1] == 1)
                        { }
                        else
                        {
                            currentBlock.x += 20;
                            xBoard += 1;
                            xBoard2 += 1;
                            xBoard3 += 1;
                            xBoard4 += 1;
                        }
                    }
                    else if (blockType == 5)
                    {
                        if (xBoard + 2 >= 10
                            || digitalBoard[yBoard2, xBoard2 + 1] == 1
                            || digitalBoard[yBoard4, xBoard4 + 1] == 1)
                        { }
                        else
                        {
                            currentBlock.x += 20;
                            xBoard += 1;
                            xBoard2 += 1;
                            xBoard3 += 1;
                            xBoard4 += 1;
                        }
                    }
                    else if (blockType == 6)
                    {
                        if (xBoard + 3 >= 10
                            || digitalBoard[yBoard2, xBoard2 + 1] == 1
                            || digitalBoard[yBoard4, xBoard4 + 1] == 1)
                        { }
                        else
                        {
                            currentBlock.x += 20;
                            xBoard += 1;
                            xBoard2 += 1;
                            xBoard3 += 1;
                            xBoard4 += 1;
                        }
                    }
                    else if (blockType == 7)
                    {
                        if (xBoard + 3 >= 10
                            || digitalBoard[yBoard2, xBoard2 + 1] == 1
                            || digitalBoard[yBoard4, xBoard4 + 1] == 1)
                        { }
                        else
                        {
                            currentBlock.x += 20;
                            xBoard += 1;
                            xBoard2 += 1;
                            xBoard3 += 1;
                            xBoard4 += 1;
                        }
                    }
                    else if (blockType == 8)
                    {
                        if (xBoard + 2 >= 10
                            || digitalBoard[yBoard, xBoard + 1] == 1
                            || digitalBoard[yBoard3, xBoard3 + 1] == 1
                            || digitalBoard[yBoard4, xBoard4 + 1] == 1)
                        { }
                        else
                        {
                            currentBlock.x += 20;
                            xBoard += 1;
                            xBoard2 += 1;
                            xBoard3 += 1;
                            xBoard4 += 1;
                        }
                    }
                    else if (blockType == 9)
                    {
                        if (xBoard + 2 >= 10
                            || digitalBoard[yBoard3, xBoard3 + 1] == 1
                            || digitalBoard[yBoard4, xBoard4 + 1] == 1)
                        { }
                        else
                        {
                            currentBlock.x += 20;
                            xBoard += 1;
                            xBoard2 += 1;
                            xBoard3 += 1;
                            xBoard4 += 1;
                        }
                    }
                }
                //Move block down
                else if (oldState.IsKeyUp(Keys.P) && newState.IsKeyDown(Keys.P) && pauseFlag == 0)
                {

                    if (blockType == 0)
                    {
                        // currentBlock.changeY();
                        if (currentBlock.y + 40 >= 380
                            || digitalBoard[yBoard + 2, xBoard] == 1)
                        { }

                        else
                        {
                            currentBlock.y += 20;
                            yBoard += 1;
                        }
                    }
                    else if (blockType == 1)
                    {
                        if (currentBlock.y + 40 >= 380
                            || digitalBoard[yBoard + 2, xBoard] == 1
                            || digitalBoard[yBoard + 2, xBoard2] == 1
                            || digitalBoard[yBoard + 2, xBoard3] == 1)
                        { }
                        else
                        {
                            currentBlock.y += 20;
                            yBoard += 1;
                            yBoard2 += 1;
                            yBoard3 += 1;
                        }
                    }
                    else if (blockType == 2)
                    {
                        if (currentBlock.y + 60 >= 380
                            || digitalBoard[yBoard3 + 2, xBoard] == 1)
                        { }
                        else
                        {
                            currentBlock.y += 20;
                            yBoard += 1;
                            yBoard2 += 1;
                            yBoard3 += 1;
                        }

                    }
                    else if (blockType == 3)
                    {
                        if (currentBlock.y + 60 >= 380
                            || digitalBoard[yBoard3 + 2, xBoard3] == 1
                            || digitalBoard[yBoard4 + 2, xBoard4] == 1)
                        { }

                        else
                        {
                            currentBlock.y += 20;
                            yBoard += 1;
                            yBoard2 += 1;
                            yBoard3 += 1;
                            yBoard4 += 1;
                        }
                    }
                    else if (blockType == 4)
                    {
                        if (currentBlock.y + 60 >= 380
                            || digitalBoard[yBoard2 + 2, xBoard2] == 1
                            || digitalBoard[yBoard4 + 2, xBoard4] == 1)
                        { }
                        else
                        {
                            currentBlock.y += 20;
                            yBoard += 1;
                            yBoard2 += 1;
                            yBoard3 += 1;
                            yBoard4 += 1;
                        }
                    }
                    else if (blockType == 5)
                    {
                        if (currentBlock.y + 40 >= 380
                            || digitalBoard[yBoard3 + 2, xBoard3] == 1
                            || digitalBoard[yBoard4 + 2, xBoard4] == 1)
                        { }
                        else
                        {
                            currentBlock.y += 20;
                            yBoard += 1;
                            yBoard2 += 1;
                            yBoard3 += 1;
                            yBoard4 += 1;
                        }
                    }
                    else if (blockType == 6)
                    {
                        if (currentBlock.y + 40 >= 380
                            || digitalBoard[yBoard + 2, xBoard] == 1
                            || digitalBoard[yBoard2 + 2, xBoard2] == 1
                            || digitalBoard[yBoard4 + 2, xBoard4] == 1)
                        { }
                        else
                        {
                            currentBlock.y += 20;
                            yBoard += 1;
                            yBoard2 += 1;
                            yBoard3 += 1;
                            yBoard4 += 1;
                        }
                    }
                    else if (blockType == 7)
                    {
                        if (currentBlock.y + 40 >= 380
                            || digitalBoard[yBoard + 2, xBoard] == 1
                            || digitalBoard[yBoard3 + 2, xBoard3] == 1
                            || digitalBoard[yBoard4 + 2, xBoard4] == 1)
                        { }
                        else
                        {
                            currentBlock.y += 20;
                            yBoard += 1;
                            yBoard2 += 1;
                            yBoard3 += 1;
                            yBoard4 += 1;
                        }
                    }
                    else if (blockType == 8)
                    {
                        if (currentBlock.y + 40 >= 380
                            || digitalBoard[yBoard + 2, xBoard] == 1
                            || digitalBoard[yBoard3 + 2, xBoard3] == 1)
                        { }
                        else
                        {
                            currentBlock.y += 20;
                            yBoard += 1;
                            yBoard2 += 1;
                            yBoard3 += 1;
                            yBoard4 += 1;
                        }
                    }
                    else if (blockType == 9)
                    {
                        if (currentBlock.y + 40 >= 380
                            || digitalBoard[yBoard + 2, xBoard] == 1
                            || digitalBoard[yBoard2 + 2, xBoard2] == 1
                            || digitalBoard[yBoard4 + 2, xBoard4] == 1)
                        { }
                        else
                        {
                            currentBlock.y += 20;
                            yBoard += 1;
                            yBoard2 += 1;
                            yBoard3 += 1;
                            yBoard4 += 1;
                        }
                    }
                }

                else if ((oldState.IsKeyUp(Keys.P) && newState.IsKeyDown(Keys.P))
                        || (oldState.IsKeyUp(Keys.A) && newState.IsKeyDown(Keys.A))
                        || (oldState.IsKeyUp(Keys.D) && newState.IsKeyDown(Keys.D))
                        || (oldState.IsKeyUp(Keys.M) && newState.IsKeyDown(Keys.M)))
                { }
                else { }
                oldState = newState;
        } // end of keyboardhandler

        bool isNextSpotFilled()
        {
            try
            {
                if (blockType == 0)
                {
                   if (digitalBoard[yBoard + 1, xBoard] == 1)
                    {
                        return true;
                    }
                   else
                    {
                        return false;
                    }
                }
                else if (blockType == 1)
                {
                    if (digitalBoard[yBoard + 1, xBoard] == 1
                        || digitalBoard[yBoard2 + 1, xBoard2] == 1
                        || digitalBoard[yBoard3 + 1, xBoard3] == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                else if (blockType == 2)
                {
                    if (digitalBoard[yBoard3 + 1, xBoard3] == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (blockType == 3)
                {
                    if (digitalBoard[yBoard3 + 1, xBoard3] == 1
                        || digitalBoard[yBoard4 + 1, xBoard4] == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (blockType == 4)
                {
                    if (digitalBoard[yBoard2 + 1, xBoard2] == 1
                        || digitalBoard[yBoard4 + 1, xBoard4] == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (blockType == 5)
                {
                    if (digitalBoard[yBoard3 + 1, xBoard3] == 1
                        || digitalBoard[yBoard4 + 1, xBoard4] == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (blockType == 6)
                {
                    if (digitalBoard[yBoard + 1, xBoard] == 1
                        || digitalBoard[yBoard2 + 1, xBoard2] == 1
                        || digitalBoard[yBoard4 + 1, xBoard4] == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (blockType == 7)
                {
                    if (digitalBoard[yBoard + 1, xBoard] == 1
                        || digitalBoard[yBoard3 + 1, xBoard3] == 1
                        || digitalBoard[yBoard4 + 1, xBoard4] == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (blockType == 8)
                {
                    if (digitalBoard[yBoard + 1, xBoard] == 1
                        || digitalBoard[yBoard3 + 1, xBoard3] == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (blockType == 9)
                {
                    if (digitalBoard[yBoard + 1, xBoard] == 1
                        || digitalBoard[yBoard2 + 1, xBoard2] == 1
                        || digitalBoard[yBoard4 + 1, xBoard4] == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch
            {
                // handle exception gracefully
            }
            return true;
        }

        int randomNumberGenerator()
        {
            /*                
            From the Microsoft Documentation:
            The following example calls the Next(Int32, Int32) method to
            generate 10 random numbers between -10 and 10. Note that the second
            argument to the method specifies the exclusive upper bound of the 
            range of random values returned by the method. In other words, the largest integer 
            that the method can return is one less than this value.

            Therefore, to randomly generate a number between 0 and 3, it must be
            called like this:
            randomNumber = RNG.Next(0, 4);          
            */
            Random RNG = new Random();
            int randomNumber = RNG.Next(0, 10);

            while (randomNumber != 0 
                && randomNumber != 1
                && randomNumber != 2 
                && randomNumber != 3
                && randomNumber != 4
                && randomNumber != 5
                && randomNumber != 6
                && randomNumber != 7
                && randomNumber != 8
                && randomNumber != 9)
            {
                randomNumber = RNG.Next(0, 11);
            }
            return randomNumber;
        }
    }
}
