using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Level_Editor
{
    public partial class Editor : Form
    {
        private int height;
        private int width;
        private bool isFuture;
        private PictureBox[,] pastMap;
        private PictureBox[,] futureMap;
        private Color currentColor;
        private bool unsavedChanges;
        private int tileSide;
        private bool selectLinks;
        private Point recepticleLoc;
        private int doorsSelected;
        private int numOfClosedDoors;

        //The string contains the part in the parenthesis in the drop down.
        private string interactable;
        //Each index corresponds to a row,col. At the row,col is the string of interactalbe if any.
        private List<string> pastInteractables;
        private List<string> futureInteractables;

        //The key is the position of the door, the value is a list of the position of the recepticles
        private Dictionary<Point, List<Point>> doorRecepticleLink;
        private Dictionary<Point, String> interactableRecepticleLink;

        /// <summary>
        /// constructs a Editor object
        /// </summary>
        /// <param name="height"> the height, in tiles, of the map</param>
        /// <param name="width"> the width, in tiles, of the map</param>
        public Editor()
        {
            InitializeComponent();
            doorsSelected = 0;
            numOfClosedDoors = 0;
            recepticleLoc = new Point(0,0);
            doorRecepticleLink = new Dictionary<Point, List<Point>>();
            interactableRecepticleLink = new Dictionary<Point, String>();
            interactable = "None";
            selectLinks = false;
            isFuture = false;
            unsavedChanges = false;
            this.Text = "Level Editor";
            currentColor = Color.Black;
            this.height = 12;
            this.width = 16;
            pastInteractables = new List<string>();
            futureInteractables = new List<string>();
            pastMap = new PictureBox[height, width];
            futureMap = new PictureBox[height, width];
            tileSide = 600 / width;
            //Create a fill map with PictureBoxes.
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    //Initialize past level and display it.
                    PictureBox p = new PictureBox();
                    p.Size = new Size(tileSide, tileSide);
                    p.Location = new Point(10 + col * tileSide, 15 + row * tileSide);
                    pastMap[row, col] = p;
                    mapGroupBox.Controls.Add(p);
                    p.MouseDown += new MouseEventHandler(tile_MouseDown);
                    p.MouseEnter += new EventHandler(tile_MouseEnter);
                    p.MouseLeave += new EventHandler(tile_MouseLeave);
                    

                    //Initialize future level but do not display it.
                    PictureBox f = new PictureBox();
                    f.Size = new Size(tileSide, tileSide);
                    f.Location = new Point(10 + col * tileSide, 15 + row * tileSide);
                    futureMap[row, col] = f;
                    f.MouseDown += new MouseEventHandler(tile_MouseDown);
                    f.MouseEnter += new EventHandler(tile_MouseEnter);
                    f.MouseLeave += new EventHandler(tile_MouseLeave);

                    //Draw walls on the border and floors on the inside by default.
                    if (row == 0 || row == height - 1 || col == 0 || col == width - 1)
                    {
                        p.BackColor = Color.Black;
                        f.BackColor = Color.Black;
                    }
                    else
                    {
                        p.BackColor = Color.Gray;
                        f.BackColor = Color.Gray;
                    }
                }
            }
            //set size of drawing box
            mapGroupBox.Size = new Size(width * tileSide + 20, height * tileSide + 25);

        }
        /// <summary>
        /// when the user selects the green PictureBox
        /// </summary>
        /// <param name="sender"> the PictureBox</param>
        /// <param name="e"> the EventArgs</param>
        private void floorPictureBox_Click(object sender, EventArgs e)
        {
            currentColor = Color.Gray;
            currentTilePictureBox.BackColor = Color.Gray;
            interactableSelectorComboBox.Text = "None";
        }
        /// <summary>
        /// when the user selects the grey PictureBox
        /// </summary>
        /// <param name="sender"> the PictureBox</param>
        /// <param name="e"> the EventArgs</param>
        private void wallPictureBox_Click(object sender, EventArgs e)
        {
            currentColor = Color.Black;
            currentTilePictureBox.BackColor = Color.Black;
            interactableSelectorComboBox.Text = "None";
        }
        /// <summary>
        /// when the user selects the Peru PictureBox
        /// </summary>
        /// <param name="sender"> the PictureBox</param>
        /// <param name="e"> the EventArgs</param>
        private void openDoorPictureBox_Click(object sender, EventArgs e)
        {
            currentColor = Color.Peru;
            currentTilePictureBox.BackColor = Color.Peru;
            interactableSelectorComboBox.Text = "None";
        }
        /// <summary>
        /// when the user selects the brown PictureBox
        /// </summary>
        /// <param name="sender"> the PictureBox</param>
        /// <param name="e"> the EventArgs</param>
        private void closedDoorPictureBox_Click(object sender, EventArgs e)
        {
            currentColor = Color.Brown;
            currentTilePictureBox.BackColor = Color.Brown;
            interactableSelectorComboBox.Text = "None";
        }
        /// <summary>
        /// when the user selects the blue PictureBox
        /// </summary>
        /// <param name="sender"> the PictureBox</param>
        /// <param name="e"> the EventArgs</param>
        private void recepticalPictureBox_Click(object sender, EventArgs e)
        {
            currentColor = Color.Blue;
            currentTilePictureBox.BackColor = Color.Blue;
            interactableSelectorComboBox.Text = "None";
        }
        /// <summary>
        /// When a new interactable is selected.
        /// </summary>
        /// <param name="sender"> The Combobox</param>
        /// <param name="e"> the EventArgs</param>
        private void interactableSelectorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            interactable = interactableSelectorComboBox.Text;
            //if it's placed on a receptIcle
            
            if (interactable.Contains("Chicken"))
            {
                currentColor = Color.Yellow;
                currentTilePictureBox.BackColor = Color.Yellow;
            }
            else if (interactable.Contains("Flower"))
            {
                currentColor = Color.Red;
                currentTilePictureBox.BackColor = Color.Red;
            }
            else if (interactable.Contains("Tree"))
            {
                currentColor = Color.Green;
                currentTilePictureBox.BackColor = Color.Green;
            }
            else if (interactable.Contains("WateringCan"))
            {
                currentColor = Color.LightGray;
                currentTilePictureBox.BackColor = Color.LightGray;
            }
        }
        /// <summary>
        /// when the user left clicks on a PictureBox, set its color property to currentColor
        /// </summary>
        /// <param name="sender"> the PictureBox</param>
        /// <param name="e"> the EventArgs</param>
        private void tile_MouseDown(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            //Only draw if you do not need to select links and it is an interactable and the tile is a floor, or it is a tile and there is not an interactable.
            if (!selectLinks && p.BackColor!= Color.Blue && ((interactable != "None" && p.BackColor == Color.Gray) || (interactable == "None" && NotAnInteractableColor(p.BackColor))))
            {
                //For the receptical
                if (currentColor == Color.Blue)
                {
                    //Place it as long as there are 2 or more doors.
                    if (numOfClosedDoors >= 2)
                    {
                        DialogResult result = MessageBox.Show("Please click on the doors you would like to link the receptical to",
                                              "Link Receptical", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                        //Make sure the user wants to place it.
                        if (result == DialogResult.OK)
                        {
                            //add empty key to dictionary when recepticle is created, even if its never used
                            //may cause issues if a recepticle is removed but like
                            //just dont do that
                            doorRecepticleLink.Add(new Point((p.Location.X - 10) / tileSide, (p.Location.Y - 15) / tileSide), new List<Point>());
                            interactableRecepticleLink.Add(new Point((p.Location.X - 10) / tileSide, (p.Location.Y - 15) / tileSide), "");
                            p.BackColor = currentColor;
                            p.Capture = false;
                            selectLinks = true;
                            currentColor = Color.White;
                            currentTilePictureBox.BackColor = Color.White;
                            recepticleLoc = p.Location;
                        }
                    }
                    
                }
                //For any other tile.
                else
                {
                    p.BackColor = currentColor;
                    p.Capture = false;
                    //Otherwise place an interactable there.         
                    if (!unsavedChanges)
                    {
                        this.Text += "*";
                    }
                    unsavedChanges = true;
                    if(p.BackColor == Color.Brown)
                    {
                        numOfClosedDoors++;
                    }
                }
                //If its an interactable, add it to the list
                if(interactable != "None")
                {
                    int row = (p.Location.Y - 15) / tileSide;
                    int col = (p.Location.X - 10) / tileSide;
                    if (isFuture)
                    {
                        futureInteractables.Add(interactable + " " + row + " " + col);
                    }
                    else
                    {
                        pastInteractables.Add(interactable + " " + row + " " + col);
                    }
                }              
            }
            //Need to select links
            else if (selectLinks)
            {
                //Make sure they click on a closed door.
                if (p.BackColor == Color.Brown && doorsSelected < 2)
                {
                    //If there is already a door at this location in the Dictionary.
                    if (doorRecepticleLink.ContainsKey(new Point((recepticleLoc.X - 10) / tileSide, (recepticleLoc.Y - 15) / tileSide)))
                    {
                        //Make sure this recepticle hasn't already been added to the door.
                        if (!doorRecepticleLink[new Point((recepticleLoc.X - 10) / tileSide, (recepticleLoc.Y - 15) / tileSide)].Contains
                            (new Point((p.Location.X - 10) / tileSide, (p.Location.Y - 15) / tileSide)))
                        {
                            doorRecepticleLink[new Point((recepticleLoc.X - 10) / tileSide, (recepticleLoc.Y - 15) / tileSide)].Add(new Point((p.Location.X - 10) / tileSide, (p.Location.Y - 15) / tileSide));
                            doorsSelected++;
                        }
                    }
                    //First time linking a receptical to this door.
                    else
                    {
                        List<Point> temp = new List<Point>();
                        temp.Add(new Point((p.Location.X - 10) / tileSide, (p.Location.Y - 15) / tileSide));
                        doorRecepticleLink[new Point((recepticleLoc.X - 10) / tileSide, (recepticleLoc.Y - 15) / tileSide)] = temp;
                        doorsSelected++;
                    }
                }
                if (doorsSelected == 2)
                {
                    MessageBox.Show("Receptical Linked to Doors", "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    doorsSelected = 0;
                    selectLinks = false;
                }
            }
            //If it's an interactable, save it to the list.
            else if (interactable != "None")
            {
                int row = (p.Location.Y - 15) / tileSide;
                int col = (p.Location.X - 10) / tileSide;
                if (p.BackColor == Color.Blue)
                {
                    interactableRecepticleLink[new Point((p.Location.X - 10) / tileSide, (p.Location.Y - 15) / tileSide)] = interactableSelectorComboBox.Text;
                    MessageBox.Show("Receptical Linked to " + interactableSelectorComboBox.Text, "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (isFuture)
                {
                    futureInteractables.Add(interactable + " " + row + " " + col);
                }
                else
                {
                    pastInteractables.Add(interactable + " " + row + " " + col);
                }
            }

        }
        private bool NotAnInteractableColor(Color color)
        {
            return (color != Color.Green && color != Color.Red && color != Color.Yellow && color != Color.LightGray);
        }
        /// <summary>
        /// When the mouse has entered the pictureBox
        /// </summary>
        /// <param name="sender"> the PictureBox</param>
        /// <param name="e"> the EventArgs</param>
        private void tile_MouseEnter(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            //You can drag if youre not selecting links and youre not placing and interactible.
            if (!selectLinks && MouseButtons == MouseButtons.Left && interactable == "None")
            {
                if(NotAnInteractableColor(p.BackColor))
                    p.BackColor = currentColor;
            }
            p.BorderStyle = BorderStyle.FixedSingle;
        }
        /// <summary>
        /// When the mouse leaves the pictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tile_MouseLeave(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            p.BorderStyle = BorderStyle.None;
        }
        /// <summary>
        /// when the user clicks the saveFileButton, open a SaveFileDialog and have the user enter a name and select a location to save it to.
        /// </summary>
        /// <param name="sender"> the Button</param>
        /// <param name="e"> the EventArgs</param>
        private void saveFileButton_Click(object sender, EventArgs e)
        {
            StreamWriter output = null;
            //try
            //(h, w{
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Level Files | *.txt";
                save.Title = "Save a level file";
                DialogResult result = save.ShowDialog();
                if (result == DialogResult.OK)
                {
                    bool[] neighbors = new bool[4];
                    int neighborNum = 0;
                    output = new StreamWriter(save.FileName);
                    //save to file each color value of the corresponding PictureBoxes and follow it with a comma
                    for (int h = 0; h < height; h++)
                    {
                        output.WriteLine("/row " + h);
                        for (int w = 0; w < width; w++)
                        {
                            //checks if wall is in this location
                            //if it is, begin check for WHICH wall
                            if (pastMap[h, w].BackColor.Equals(Color.Black))
                            {
                                //check if wall is on edge
                                //check each individual edge and if it's a corner
                                if (h == 0)
                                {
                                    if (w == 0)
                                        output.Write("ExtCorner 0 ");
                                    else if (w == width - 1)
                                        output.Write("ExtCorner 2 ");
                                    else
                                        output.Write("ExtWallHoriz 0 ");
                                }
                                else if (w == 0)
                                {
                                    if (h == height - 1)
                                        output.Write("ExtCorner 1 ");
                                    else
                                        output.Write("ExtWallVert 0 ");
                                }
                                else if(w == width - 1)
                                {
                                    if (h == height - 1)
                                        output.Write("ExtCorner 3 ");
                                    else
                                        output.Write("ExtWallVert 2 ");
                                }
                                else if(h == height - 1)
                                {
                                    output.Write("ExtWallHoriz 1 ");
                                }
                                //if this wall isnt an edge
                                else
                                {
                                    //fill array of bools that dictate if there is a wall in each neigboring location
                                    neighborNum = 0;
                                    neighbors[0] = (pastMap[h - 1, w].BackColor.Equals(Color.Black));
                                    neighbors[1] = (pastMap[h, w + 1].BackColor.Equals(Color.Black));
                                    neighbors[2] = (pastMap[h + 1, w].BackColor.Equals(Color.Black));
                                    neighbors[3] = (pastMap[h, w - 1].BackColor.Equals(Color.Black));

                                    //creates a number that dictates how many neighboring walls there is to this one
                                    for(int i = 0; i < 4; i++)
                                    {
                                        if (neighbors[i])
                                            neighborNum++;
                                    }

                                    //if there are no neighbors, make it a horizontal wall
                                    if (neighborNum == 0)
                                    {
                                        output.Write("IntWallHoriz 0 ");
                                    }
                                    //if there is one neighbor, determine if it is:
                                    //above / below (vertical) OR
                                    //left / right (horizontal)
                                    else if(neighborNum == 1)
                                    {
                                        if (neighbors[0] || neighbors[2])
                                            output.Write("IntWallVert 0 ");
                                        else
                                            output.Write("IntWallHoriz 0 ");
                                    }
                                    //if two neighbors, determine:
                                    //if above and below (vertical)
                                    //if left and right (horizontal)
                                    //if otherwise, it is a corner. check each orientation.
                                    else if(neighborNum == 2)
                                    {
                                        if (neighbors[0] && neighbors[2])
                                            output.Write("IntWallVert 0 ");
                                        else if(neighbors[1] && neighbors[3])
                                            output.Write("IntWallHoriz 0 ");
                                        else if (neighbors[1] && neighbors[2])
                                            output.Write("IntWallCorner 0 ");
                                        else if (neighbors[2] && neighbors[3])
                                            output.Write("IntWallCorner 2 ");
                                        else if (neighbors[0] && neighbors[3])
                                            output.Write("IntWallCorner 3 ");
                                        else if (neighbors[0] && neighbors[1])
                                            output.Write("IntWallCorner 1 ");
                                    }
                                    //if there are three neighbors, a T-block is needed
                                    //check which neighbor ISNT there, place corresponding tile
                                    else if(neighborNum == 3)
                                    {
                                        if (!neighbors[0])
                                            output.Write("TWallHoriz 0 ");
                                        else if (!neighbors[1])
                                            output.Write("TWallVert 1 ");
                                        else if (!neighbors[2])
                                            output.Write("TWallHoriz 1 ");
                                        else if (!neighbors[3])
                                            output.Write("TWallVert 0 ");
                                    }
                                    else
                                    {
                                        output.Write("IntWallCross 0 ");
                                    }
                                }
                            }
                            //if not a wall, check if its an OPEN door
                            else if (pastMap[h, w].BackColor.Equals(Color.Peru))
                            {
                                if(h == 0)
                                {
                                    if (pastMap[h, w + 1].BackColor.Equals(Color.Peru))
                                    {
                                        output.Write("OpenDoorExtHoriz 0 ");
                                    }
                                    else if (pastMap[h, w - 1].BackColor.Equals(Color.Peru))
                                    {
                                        output.Write("OpenDoorExtHoriz 2 ");
                                    }
                                }
                                else if (h == height - 1)
                                {
                                    if (pastMap[h, w + 1].BackColor.Equals(Color.Peru))
                                    {
                                        output.Write("OpenDoorExtHoriz 1 ");
                                    }
                                    else if (pastMap[h, w - 1].BackColor.Equals(Color.Peru))
                                    {
                                        output.Write("OpenDoorExtHoriz 3 ");
                                    }
                                }
                                else if (w == 0)
                                {
                                    if (pastMap[h - 1, w].BackColor.Equals(Color.Peru))
                                    {
                                        output.Write("OpenDoorExtVert 0 ");
                                    }
                                    else if (pastMap[h + 1, w].BackColor.Equals(Color.Peru))
                                    {
                                        output.Write("OpenDoorExtVert 1 ");
                                    }
                                }
                                else if (w == width - 1)
                                {
                                    if (pastMap[h - 1, w].BackColor.Equals(Color.Peru))
                                    {
                                        output.Write("OpenDoorExtVert 2 ");
                                    }
                                    else if (pastMap[h + 1, w].BackColor.Equals(Color.Peru))
                                    {
                                        output.Write("OpenDoorExtVert 3 ");
                                    }
                                }
                                else
                                {
                                    //fill array of bools that dictate if there is a DOOR in each neigboring location
                                    neighbors[0] = (pastMap[h - 1, w].BackColor.Equals(Color.Peru));
                                    neighbors[1] = (pastMap[h, w + 1].BackColor.Equals(Color.Peru));
                                    neighbors[2] = (pastMap[h + 1, w].BackColor.Equals(Color.Peru));
                                    neighbors[3] = (pastMap[h, w - 1].BackColor.Equals(Color.Peru));

                                    if (neighbors[0])
                                        output.Write("OpenDoorIntVert 0 ");
                                    else if (neighbors[1])
                                        output.Write("OpenDoorIntHoriz 0 ");
                                    else if (neighbors[2])
                                        output.Write("OpenDoorIntVert 1 ");
                                    else if (neighbors[3])
                                        output.Write("OpenDoorIntHoriz 2 ");
                                }
                            }
                            //if not an OPEN door, check if its a CLOSED door
                            else if (pastMap[h, w].BackColor.Equals(Color.Brown))
                            {
                                if (h == 0)
                                {
                                    if (pastMap[h, w + 1].BackColor.Equals(Color.Brown))
                                    {
                                        output.Write("LockedDoorExtHoriz 0 ");
                                    }
                                    else if (pastMap[h, w - 1].BackColor.Equals(Color.Brown))
                                    {
                                        output.Write("LockedDoorExtHoriz 2 ");
                                    }
                                }
                                else if (h == height - 1)
                                {
                                    if (pastMap[h, w + 1].BackColor.Equals(Color.Brown))
                                    {
                                        output.Write("LockedDoorExtHoriz 1 ");
                                    }
                                    else if (pastMap[h, w - 1].BackColor.Equals(Color.Brown))
                                    {
                                        output.Write("LockedDoorExtHoriz 3 ");
                                    }
                                }
                                else if (w == 0)
                                {
                                    if (pastMap[h - 1, w].BackColor.Equals(Color.Brown))
                                    {
                                        output.Write("LockedDoorExtVert 0 ");
                                    }
                                    else if (pastMap[h + 1, w].BackColor.Equals(Color.Brown))
                                    {
                                        output.Write("LockedDoorExtVert 1 ");
                                    }
                                }
                                else if (w == width - 1)
                                {
                                    if (pastMap[h - 1, w].BackColor.Equals(Color.Brown))
                                    {
                                        output.Write("LockedDoorExtVert 2 ");
                                    }
                                    else if (pastMap[h + 1, w].BackColor.Equals(Color.Brown))
                                    {
                                        output.Write("LockedDoorExtVert 3 ");
                                    }
                                }
                                else
                                {
                                    //fill array of bools that dictate if there is a DOOR in each neigboring location
                                    neighbors[0] = (pastMap[h - 1, w].BackColor.Equals(Color.Brown));
                                    neighbors[1] = (pastMap[h, w + 1].BackColor.Equals(Color.Brown));
                                    neighbors[2] = (pastMap[h + 1, w].BackColor.Equals(Color.Brown));
                                    neighbors[3] = (pastMap[h, w - 1].BackColor.Equals(Color.Brown));

                                    if (neighbors[0])
                                        output.Write("LockedDoorIntVert 0 ");
                                    else if (neighbors[1])
                                        output.Write("LockedDoorIntHoriz 0 ");
                                    else if (neighbors[2])
                                        output.Write("LockedDoorIntVert 1 ");
                                    else if (neighbors[3])
                                        output.Write("LockedDoorIntHoriz 2 ");
                                }
                            }
                            else
                                output.Write("Floor 0 ");

                            //repeat the WHOLE thing for the future
                            if (futureMap[h, w].BackColor.Equals(Color.Black))
                            {
                                //check if wall is on edge
                                //check each individual edge and if it's a corner
                                if (h == 0)
                                {
                                    if (w == 0)
                                        output.WriteLine("ExtCorner 0 ");
                                    else if (w == width - 1)
                                        output.WriteLine("ExtCorner 2 ");
                                    else
                                        output.WriteLine("ExtWallHoriz 0");
                                }
                                else if (w == 0)
                                {
                                    if (h == height - 1)
                                        output.WriteLine("ExtCorner 1");
                                    else
                                        output.WriteLine("ExtWallVert 0");
                                }
                                else if (w == width - 1)
                                {
                                    if (h == height - 1)
                                        output.WriteLine("ExtCorner 3");
                                    else
                                        output.WriteLine("ExtWallVert 2");
                                }
                                else if (h == height - 1)
                                {
                                    output.WriteLine("ExtWallHoriz 1");
                                }
                                //if this wall isnt an edge
                                else
                                {
                                    //fill array of bools that dictate if there is a wall in each neigboring location
                                    neighborNum = 0;
                                    neighbors[0] = (futureMap[h - 1, w].BackColor.Equals(Color.Black));
                                    neighbors[1] = (futureMap[h, w + 1].BackColor.Equals(Color.Black));
                                    neighbors[2] = (futureMap[h + 1, w].BackColor.Equals(Color.Black));
                                    neighbors[3] = (futureMap[h, w - 1].BackColor.Equals(Color.Black));

                                    //creates a number that dictates how many neighboring walls there is to this one
                                    for (int i = 0; i < 4; i++)
                                    {
                                        if (neighbors[i])
                                            neighborNum++;
                                    }

                                    //if there are no neighbors, make it a horizontal wall
                                    if (neighborNum == 0)
                                    {
                                        output.WriteLine("IntWallHoriz 0");
                                    }
                                    //if there is one neighbor, determine if it is:
                                    //above / below (vertical) OR
                                    //left / right (horizontal)
                                    else if (neighborNum == 1)
                                    {
                                        if (neighbors[0] || neighbors[2])
                                            output.WriteLine("IntWallVert 0");
                                        else
                                            output.WriteLine("IntWallHoriz 0");
                                    }
                                    //if two neighbors, determine:
                                    //if above and below (vertical)
                                    //if left and right (horizontal)
                                    //if otherwise, it is a corner. check each orientation.
                                    else if (neighborNum == 2)
                                    {
                                        if (neighbors[0] && neighbors[2])
                                            output.WriteLine("IntWallVert 0");
                                        else if (neighbors[1] && neighbors[3])
                                            output.WriteLine("IntWallHoriz 0");
                                        else if (neighbors[1] && neighbors[2])
                                            output.WriteLine("IntWallCorner 0");
                                        else if (neighbors[2] && neighbors[3])
                                            output.WriteLine("IntWallCorner 2");
                                        else if (neighbors[0] && neighbors[3])
                                            output.WriteLine("IntWallCorner 3");
                                        else if (neighbors[0] && neighbors[1])
                                            output.WriteLine("IntWallCorner 1");
                                    }
                                    //if there are three neighbors, a T-block is needed
                                    //check which neighbor ISNT there, place corresponding tile
                                    else if (neighborNum == 3)
                                    {
                                        if (!neighbors[0])
                                            output.WriteLine("TWallHoriz 0");
                                        else if (!neighbors[1])
                                            output.WriteLine("TWallVert 2");
                                        else if (!neighbors[2])
                                            output.WriteLine("TWallHoriz 2");
                                        else if (!neighbors[3])
                                            output.WriteLine("TWallVert 0");
                                    }
                                    else
                                    {
                                        output.WriteLine("IntWallCross 0");
                                    }
                                }
                            }
                            //if not, check if its an OPEN door
                            else if (futureMap[h, w].BackColor.Equals(Color.Peru))
                            {
                                if (h == 0)
                                {
                                    if (futureMap[h, w + 1].BackColor.Equals(Color.Peru))
                                    {
                                        output.WriteLine("OpenDoorExtHoriz 0");
                                    }
                                    else if (futureMap[h, w - 1].BackColor.Equals(Color.Peru))
                                    {
                                        output.WriteLine("OpenDoorExtHoriz 2");
                                    }
                                }
                                else if (h == height - 1)
                                {
                                    if (futureMap[h, w + 1].BackColor.Equals(Color.Peru))
                                    {
                                        output.WriteLine("OpenDoorExtHoriz 1");
                                    }
                                    else if (futureMap[h, w - 1].BackColor.Equals(Color.Peru))
                                    {
                                        output.WriteLine("OpenDoorExtHoriz 3");
                                    }
                                }
                                else if (w == 0)
                                {
                                    if (futureMap[h - 1, w].BackColor.Equals(Color.Peru))
                                    {
                                        output.WriteLine("OpenDoorExtVert 0");
                                    }
                                    else if (futureMap[h + 1, w].BackColor.Equals(Color.Peru))
                                    {
                                        output.WriteLine("OpenDoorExtVert 1");
                                    }
                                }
                                else if (w == width - 1)
                                {
                                    if (futureMap[h - 1, w].BackColor.Equals(Color.Peru))
                                    {
                                        output.WriteLine("OpenDoorExtVert 2");
                                    }
                                    else if (futureMap[h + 1, w].BackColor.Equals(Color.Peru))
                                    {
                                        output.WriteLine("OpenDoorExtVert 3");
                                    }
                                }
                                else
                                {
                                    //fill array of bools that dictate if there is a DOOR in each neigboring location
                                    neighbors[0] = (futureMap[h - 1, w].BackColor.Equals(Color.Peru));
                                    neighbors[1] = (futureMap[h, w + 1].BackColor.Equals(Color.Peru));
                                    neighbors[2] = (futureMap[h + 1, w].BackColor.Equals(Color.Peru));
                                    neighbors[3] = (futureMap[h, w - 1].BackColor.Equals(Color.Peru));

                                    if (neighbors[0])
                                        output.WriteLine("OpenDoorIntVert 0");
                                    else if (neighbors[1])
                                        output.WriteLine("OpenDoorIntHoriz 0");
                                    else if (neighbors[2])
                                        output.WriteLine("OpenDoorIntVert 1");
                                    else if (neighbors[3])
                                        output.WriteLine("OpenDoorIntHoriz 2");
                                }
                            }
                            //if not, check if its a CLOSED door
                            else if (futureMap[h, w].BackColor.Equals(Color.Brown))
                            {
                                if (h == 0)
                                {
                                    if (futureMap[h, w + 1].BackColor.Equals(Color.Brown))
                                    {
                                        output.WriteLine("LockedDoorExtHoriz 0");
                                    }
                                    else if (futureMap[h, w - 1].BackColor.Equals(Color.Brown))
                                    {
                                        output.WriteLine("LockedDoorExtHoriz 2");
                                    }
                                }
                                else if (h == height - 1)
                                {
                                    if (futureMap[h, w + 1].BackColor.Equals(Color.Brown))
                                    {
                                        output.WriteLine("LockedDoorExtHoriz 1");
                                    }
                                    else if (futureMap[h, w - 1].BackColor.Equals(Color.Brown))
                                    {
                                        output.WriteLine("LockedDoorExtHoriz 3");
                                    }
                                }
                                else if (w == 0)
                                {
                                    if (futureMap[h - 1, w].BackColor.Equals(Color.Brown))
                                    {
                                        output.WriteLine("LockedDoorExtVert 0");
                                    }
                                    else if (futureMap[h + 1, w].BackColor.Equals(Color.Brown))
                                    {
                                        output.WriteLine("LockedDoorExtVert 1");
                                    }
                                }
                                else if (w == width - 1)
                                {
                                    if (futureMap[h - 1, w].BackColor.Equals(Color.Brown))
                                    {
                                        output.WriteLine("LockedDoorExtVert 2");
                                    }
                                    else if (futureMap[h + 1, w].BackColor.Equals(Color.Brown))
                                    {
                                        output.WriteLine("LockedDoorExtVert 3");
                                    }
                                }
                                else
                                {
                                    //fill array of bools that dictate if there is a DOOR in each neigboring location
                                    neighbors[0] = (futureMap[h - 1, w].BackColor.Equals(Color.Brown));
                                    neighbors[1] = (futureMap[h, w + 1].BackColor.Equals(Color.Brown));
                                    neighbors[2] = (futureMap[h + 1, w].BackColor.Equals(Color.Brown));
                                    neighbors[3] = (futureMap[h, w - 1].BackColor.Equals(Color.Brown));

                                    if (neighbors[0])
                                        output.WriteLine("LockedDoorIntVert 0");
                                    else if (neighbors[1])
                                        output.WriteLine("LockedDoorIntHoriz 0");
                                    else if (neighbors[2])
                                        output.WriteLine("LockedDoorIntVert 1");
                                    else if (neighbors[3])
                                        output.WriteLine("LockedDoorIntHoriz 2");
                                }
                            }
                            else
                                output.WriteLine("Floor 0");
                        }
                    }

                    bool areInteractables = false, areRecepticles = false;

                    foreach(PictureBox p in pastMap)
                    {
                        if (p.BackColor == Color.Blue)
                            areRecepticles = true;
                        else if (p.BackColor == Color.Yellow || p.BackColor == Color.Green || p.BackColor == Color.Red || p.BackColor == Color.LightGray)
                            areInteractables = true;
                    }

                    if(!(areInteractables && areRecepticles))
                    {
                        foreach (PictureBox p in futureMap)
                        {
                            if (p.BackColor == Color.Blue)
                                areRecepticles = true;
                            else if (p.BackColor == Color.Yellow || p.BackColor == Color.Green || p.BackColor == Color.Red || p.BackColor == Color.LightGray)
                                areInteractables = true;
                        }
                    }

                    if(areInteractables)
                    {
                        output.WriteLine("/Interactables");

                        foreach(String s in futureInteractables)
                        {
                            output.WriteLine(s);
                        }
                    }

                    if (areRecepticles)
                    {
                        output.WriteLine("/recepticles");

                        for (int h = 0; h < height; h++)
                        {
                            for (int w = 0; w < width; w++)
                            {
                                if (pastMap[h, w].BackColor == Color.Blue)
                                {
                                    //info about recepticle
                                    output.Write("Past " + interactableRecepticleLink[new Point(w, h)] + " " + h + " " + w);

                                    //info about doors

                                    foreach (Point p in doorRecepticleLink[new Point(w, h)])
                                    {
                                        output.Write(" " + p.X + "," + p.Y);
                                    }
                                    //to go to next line
                                    output.WriteLine();
                                }

                                //if the recepticle is in the future
                                if (futureMap[h, w].BackColor == Color.Blue)
                                {
                                    //info about recepticle
                                    output.Write("Future " + interactableRecepticleLink[new Point(w, h)] + " " + h + " " + w);

                                    //info about doors

                                    foreach (Point p in doorRecepticleLink[new Point(w, h)])
                                    {
                                        output.Write(" " + p.X + "," + p.Y);
                                    }

                                    //to go to next line
                                    output.WriteLine();
                                }
                            }
                        }
                    }
                }

                //document unsaved changes
                this.Text = "Level Editor - " +save.FileName;
            //}
            //catch(Exception oops)
            //{
            //    Console.WriteLine("Error saving file: " +oops.Message);
            //}
            //finally
            //{
                if(output != null)
                {
                    output.Close();
                    MessageBox.Show("File saved successfully", "File saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    unsavedChanges = false;
                }
            //}         
        }
        /// <summary>
        /// when the user clicks the loadFileButton, dispose old map of PictureBoxes and load new file.
        /// </summary>
        /// <param name="sender"> the Button</param>
        /// <param name="e"> the EventArgs</param>
        private void loadFileButton_Click(object sender, EventArgs e)
        {
            // need to go through array of Picture Box's and physically removed them from the Editor, simply deleting, or re-assigning the pointer to
            // map doesn't actually remove them.
            for(int i = 0; i< height; i++)
            {
                for(int j = 0; j< width; j++)
                {
                    if (!isFuture)
                        pastMap[i, j].Dispose();
                    else
                        futureMap[i, j].Dispose();
                }
            }
            LoadFile();     
        }
        /// <summary>
        /// when the user clicks the loadFileButton, open a OpenFileDiaglog and have the user select a .Level file to open.
        /// </summary>
        private void LoadFile()
        {
            mapGroupBox.Size = new Size(width * tileSide + 20, height * tileSide + 25);
            StreamReader input = null;
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Level Files | *.txt";
                open.Title = "Load a level file";
                DialogResult result = open.ShowDialog();
                string[] line;
                if (result == DialogResult.OK)
                {
                    input = new StreamReader(open.FileName);
                    pastMap = new PictureBox[height, width];
                    futureMap = new PictureBox[height, width];

                    //take the next color value in the text file and give the corresponding PictureBox that color value...
                    //also position the PictureBox appropriately
                    for(int h = 0; h < 12; h++)
                    {
                        for (int w = 0; w < 16; w++)
                        {
                            line = input.ReadLine().Split();
                            if(line[0].ToLower() == "/row")
                            {
                                w--;
                                continue;
                            }

                            PictureBox p = new PictureBox();
                            p.Size = new Size(tileSide, tileSide);
                            p.Location = new Point(10 + w * tileSide, 15 + h * tileSide);
                            pastMap[h, w] = p;
                            mapGroupBox.Controls.Add(p);

                            if (line[0] == "OpenDoorExtVert" || line[0] == "OpenDoorExtHoriz" || line[0] == "OpenDoorIntVert" || line[0] == "OpenDoorIntHoriz" || line[0] == "door")
                                p.BackColor = Color.Peru;
                            else if(line[0] == "LockedDoorExtVert" || line[0] == "LockedDoorExtHoriz" || line[0] == "LockedDoorIntVert" || line[0] == "LockedDoorIntHoriz")
                                p.BackColor = Color.Brown;
                            else if (line[0].ToLower() == "floor")
                                p.BackColor = Color.Gray;
                            else
                                p.BackColor = Color.Black;

                            PictureBox f = new PictureBox();
                            f.Size = new Size(tileSide, tileSide);
                            f.Location = new Point(10 + w * tileSide, 15 + h * tileSide);
                            futureMap[h, w] = f;
                            mapGroupBox.Controls.Add(f);

                            if (line[2] == "OpenDoorExtVert" || line[2] == "OpenDoorExtHoriz" || line[2] == "OpenDoorIntVert" || line[2] == "OpenDoorIntHoriz" || line[2] == "door")
                                p.BackColor = Color.Peru;
                            else if (line[2] == "LockedDoorExtVert" || line[2] == "LockedDoorExtHoriz" || line[2] == "LockedDoorIntVert" || line[2] == "LockedDoorIntHoriz")
                                p.BackColor = Color.Brown;
                            else if (line[2].ToLower() == "floor")
                                f.BackColor = Color.Gray;
                            else
                                f.BackColor = Color.Black;

                            p.MouseDown += new MouseEventHandler(tile_MouseDown);
                            p.MouseEnter += new EventHandler(tile_MouseEnter);
                            f.MouseDown += new MouseEventHandler(tile_MouseDown);
                            f.MouseEnter += new EventHandler(tile_MouseEnter);
                        }
                    }

                    input.ReadLine();
                    string interName = "";
                    string[] details;
                    while(interName != "/recepticles")
                    {
                        interName = input.ReadLine();
                        details = interName.Split();
                        if(details[0] == "Chicken")
                        {
                            futureMap[int.Parse(details[2]), int.Parse(details[3])].BackColor = Color.Yellow;
                        }
                        else if (details[0] == "Flower")
                        {
                            futureMap[int.Parse(details[2]), int.Parse(details[3])].BackColor = Color.Red;
                        }
                        else if (details[0] == "WateringCan")
                        {
                            futureMap[int.Parse(details[2]), int.Parse(details[3])].BackColor = Color.LightGray;
                        }
                        else if (details[0] == "Tree")
                        {
                            futureMap[int.Parse(details[2]), int.Parse(details[3])].BackColor = Color.Green;
                        }
                    }

                    string recept = "";
                    while(recept != null)
                    {
                        recept = input.ReadLine();
                        details = recept.Split();

                        if(details[0] == "Past")
                        {
                            pastMap[int.Parse(details[2]), int.Parse(details[3])].BackColor = Color.Blue;
                        }
                        else if (details[0] == "Future")
                        {
                            futureMap[int.Parse(details[2]), int.Parse(details[3])].BackColor = Color.Blue;
                        }
                    }

                    this.Text = "Level Editor - " + open.FileName;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading file: " + e.Message);
            }
            finally
            {
                if (input != null)
                {
                    input.Close();
                    MessageBox.Show("File loaded successfully", "File loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    unsavedChanges = false;
                }
            }
        }

        /// <summary>
        /// when the user closes the window, if there are unsaved changes, ask the user if they would like to proceed.
        /// </summary>
        /// <param name="sender"> the close button</param>
        /// <param name="e"> the EventArgs</param>
        private void Editor_FormClosing(object sender, EventArgs e)
        {
            if (unsavedChanges)
            {
                DialogResult result = MessageBox.Show("There are unsaved changes. Are you sure you want to quit?", "Unsaved changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.No)
                {
                    FormClosingEventArgs a = (FormClosingEventArgs)e;
                    a.Cancel = true;
                } 
            }
        }
        /// <summary>
        /// When the togglePastFuture button gets pressed, switch the time state and display the corresponding level.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void togglePastFuture_Click(object sender, EventArgs e)
        {
            //Loop through and remove all the current Pictures boxes and remove them, then add the other time states Picture boxes to
            //the map group box.
            for(int row = 0; row<height; row++)
            {
                for(int col = 0; col<width; col++)
                {
                    if (!isFuture)
                    {
                        mapGroupBox.Controls.Remove(pastMap[row, col]);
                        mapGroupBox.Controls.Add(futureMap[row, col]);
                    }
                    else
                    {
                        mapGroupBox.Controls.Remove(futureMap[row, col]);
                        mapGroupBox.Controls.Add(pastMap[row, col]);
                    }
                }
            }
            if (!isFuture)
            {
                isFuture = true;
                togglePastFuture.Text = "Past Level";
                mapGroupBox.Text = "Future Level Map";
            }
            else
            {
                isFuture = false;
                togglePastFuture.Text = "Future Level";
                mapGroupBox.Text = "Past Level Map";
            }
        }

        private void recepticalLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
