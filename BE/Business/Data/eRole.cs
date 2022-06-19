﻿using System.ComponentModel;

namespace Business.Data
{
    public enum eRole
    {
        [Description(Role.Admin)]
        Admin = 1,

        [Description(Role.EditorQTNS)]
        EditorQTNS,

        [Description(Role.EditorQTDA)]
        EditorQTDA,

        [Description(Role.Viewer)]
        Viewer
    }

    public class Role
    {
        #region Role
        public const string Admin = "admin";
        public const string EditorQTNS = "editor-qtns";
        public const string EditorQTDA = "editor-qtda";
        public const string Viewer = "viewer";
        #endregion
    }
}
