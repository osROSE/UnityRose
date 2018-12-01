#region License

/**
 * Copyright (C) 2012 Jack Wakefield
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

#endregion

namespace Revise.Files.HLP {
    /// <summary>
    /// Represents a help page.
    /// </summary>
    public class HelpPage {
        #region Properties

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        public string Title {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the page content.
        /// </summary>
        public string Content {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpPage"/> class.
        /// </summary>
        public HelpPage() {
            Title = string.Empty;
            Content = string.Empty;
        }
    }
}