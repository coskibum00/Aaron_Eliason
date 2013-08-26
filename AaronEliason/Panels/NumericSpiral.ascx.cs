using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace AaronEliason.UI
{
    /// <summary>
    /// Creates spiral display of integers based on an entered
    /// positive integer.  The display counts from 0 up to the
    /// value entered with the spiral rotating clockwise as it 
    /// moves outward.
    /// </summary>
    public partial class NumericSpiral : UserControl
    {
        #region Private Members

        /// <summary>
        /// Stores the number of rows and columns the HTML table will have
        /// to hold all of the numbers
        /// </summary>
        private int _spiralSize = 0;

        /// <summary>
        /// Stores arrays of arrays which represent the values that will be
        /// displayed within the HTML table
        /// </summary>
        private string[][] _spiralMap;

        /// <summary>
        /// Defines the length of the spiral that will be written.  This
        /// is public, so that instances of this control can be used without
        /// needing to first collect input.
        /// </summary>
        private int? _inputValue { get; set; }

        #endregion

        #region UserControl Overrides

        /// <summary>
        /// Attempt to build the sprial if an input value
        /// has been entered.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (_inputValue.HasValue)
                buildSpiral();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Capture event and trigger creation of the number spiral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void onCreateOutput_Clicked(object sender, EventArgs args)
        {
            buildSpiral();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Conducts the steps necessary to validate the input and create
        /// a spiral shape of numbers
        /// </summary>
        private void buildSpiral()
        {
            // Validate value
            if (validateInput())
            {
                calculateSpiralSize();
                if (fillDataArrays())
                {
                    displayNumericSpiral();
                }
            }
        }

        /// <summary>
        /// Validates that a value was entered,it's a positive number,
        /// and that it's a whole number.
        /// </summary>
        /// <returns></returns>
        private bool validateInput()
        {
            bool errorFound = false;

            if (string.IsNullOrEmpty(IntegerInput.Text))
            {
                createError("Please enter value to display");
                errorFound = true;
            }

            decimal inputDecimal = decimal.MinValue;
            if (decimal.TryParse(IntegerInput.Text, out inputDecimal) == false)
            {
                // Report that input could not be read
                createError("Unable to parse value as a number");
                errorFound = true;
            }

            // Check for a positive whole number
            if (inputDecimal < 0
                || (inputDecimal % 1 != 0))
            {
                // Report that number needs to be positive
                createError("Please enter a positive whole number");
                errorFound = true;
            }

            // If no errors were found then assign entered input to 
            // _inputValue property for use throughout the control.
            if (errorFound == false)
            {
                _inputValue = Convert.ToInt32(inputDecimal);
            }

            return !errorFound;
        }

        /// <summary>
        /// Based upon the input, decide how large of a grid is needed
        /// to display all of the numbers.
        /// </summary>
        private void calculateSpiralSize()
        {
            // Add one to account for number zero appearing in the output
            double squareRoot = Math.Sqrt(_inputValue.GetValueOrDefault(0) + 1);

            // Use the next highest whole number to ensure that we create enough
            // columns to display the outermost ring of the spiral
            int.TryParse(Math.Ceiling(squareRoot).ToString(), out _spiralSize);
        }

        /// <summary>
        /// Identifies the proper order and placement for each number from 0
        /// to the entered value such that it will create a spiral shape starting 
        /// in the middle with 0 and working it's way outward while rotating clockwise.
        /// The numbers are stored in an array of arrays where the top level array
        /// represents rows in an HTML table and the inner arrays the table cells
        /// for each row.
        /// </summary>
        /// </summary>
        private bool fillDataArrays()
        {
            bool successfullyFilledArrays = true;

            if (_spiralSize < 1)
                return false;

            // Initialize x/y axis point at center of columns
            // and rows in the output display
            int level = 1;
            int value = 0;
            int x, y;
            int initialX, initialY;
            // It's key to identify the coordinates for the first number
            // to start the spiral, and that's why this picks the original
            // x and y coordinates by incrementing the width and height of
            // the display grid's overall width and height.
            initialX = initialY = x = y = (_spiralSize % 2 == 0)
                ? _spiralSize / 2 - 1
                : (int)Math.Floor((decimal)(_spiralSize / 2));

            try
            {
                // Initialize arrays
                _spiralMap = new string[_spiralSize][];
                for (int i = 0; i < _spiralSize; i++)
                {
                    _spiralMap[i] = new string[_spiralSize];
                }

                // Seed the spiral with the initial value
                _spiralMap[x][y] += value.ToString();
                value++;

                for (int i = 0; i <= level && value <= _inputValue; i++)
                {
                    // Move right until we've reach threshold for this level
                    while (x < initialX + level && x + 1 < _spiralSize && value <= _inputValue.Value)
                    {
                        x++;
                        if (value <= _inputValue.Value)
                        {
                            _spiralMap[x][y] += value.ToString();
                            value++;
                        }
                    }

                    // Move down until we've reach threshold for this level
                    while (y < initialY + level && value <= _inputValue.Value)
                    {
                        y++;
                        if (value <= _inputValue.Value)
                        {
                            _spiralMap[x][y] += value.ToString();
                            value++;
                        }
                    }


                    // Move left until we've reach threshold for this level
                    while (x > initialX - level && value <= _inputValue.Value)
                    {
                        x--;
                        if (value <= _inputValue.Value)
                        {
                            _spiralMap[x][y] += value.ToString();
                            value++;
                        }
                    }

                    // Move up until we've reach threshold for this level
                    while (y > initialY - level && value <= _inputValue.Value)
                    {
                        y--;
                        if (value <= _inputValue.Value)
                        {
                            _spiralMap[x][y] += value.ToString();
                            value++;
                        }
                    }

                    // Move to the next level outward
                    level++;
                }
            }
            catch (Exception ex)
            {
                createError(ex.Message + ":" + value + ":" + x + "'" + y);
                successfullyFilledArrays = false;
            }

            return successfullyFilledArrays;
        }

        /// <summary>
        /// Draws the spiral shape for the entered number starting in the
        /// middle and working it's way outward while rotating clockwise
        /// </summary>
        private void displayNumericSpiral()
        {
            Output.Controls.Clear();

            if (_spiralMap != null)
            {
                HtmlTable display = new HtmlTable();
                display.Attributes["class"] = "result";
                for (int r = 0; r < _spiralSize; r++)
                {
                    HtmlTableRow row = new HtmlTableRow();
                    for (int c = 0; c < _spiralSize; c++)
                    {
                        // Verify that this part of spiral has a value
                        if (_spiralMap[c] != null && _spiralMap[c][r] != null)
                            createResultCell(row, _spiralMap[c][r].ToString());
                        else
                            createResultCell(row, "&nbsp;");
                    }
                    display.Rows.Add(row);
                }

                Output.Controls.Add(display);
            }
        }

        /// <summary>
        /// Create table cell to hold the specified value
        /// </summary>
        /// <param name="displayRow"></param>
        /// <param name="value"></param>
        private void createResultCell(HtmlTableRow displayRow, string value)
        {
            displayRow.Cells.Add(new HtmlTableCell() { InnerHtml = value });
        }

        /// <summary>
        /// Add a new ASP.NET Label to display the validation/error message
        /// </summary>
        /// <param name="errorText"></param>
        private void createError(string errorText)
        {
            // Precede error with a break, so that the errors appear stacked.
            if (ValidationErrors.Controls.Count >= 1)
                ValidationErrors.Controls.Add(new HtmlGenericControl("br"));
            
            ValidationErrors.Controls.Add(new Label() { Text = errorText });
            ValidationErrors.Visible = true;
        }

        #endregion
    }
}