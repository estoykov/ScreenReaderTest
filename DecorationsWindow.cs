﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenReaderTest
{
    public class DecorationsWindow : CustomNativeWindow
    {
        public static Color KeyColor = Color.Fuchsia;

        public DecorationsWindow(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            InitWindow();
            AddDecorations();
        }

        public MainWindow MainWindow { get; }

        public Panel ControlPanel { get; private set; }
        public Button ZOrderButton { get; private set; }
        public Label ZOrderLabel { get; private set; }

        private void InitWindow()
        {
            var createParams = new CreateParams();
            createParams.Caption = "Decorations window";
            CreateHandle(createParams);
            Utils.SetChildWindowStyles(Handle);
            Utils.SetLayeredWindowStyles(Handle, KeyColor);
        }

        private void AddDecorations()
        {
            ZOrderButton = new Button();
            ZOrderButton.Text = "Z-order";
            ZOrderButton.Dock = DockStyle.Left;
            ZOrderButton.Click += OnZOrderButtonClick;
            ZOrderLabel = new Label();
            ZOrderLabel.Dock = DockStyle.Left;
            ZOrderLabel.AutoSize = true;
            UpdateZOrderText();
            ControlPanel = new Panel();
            ControlPanel.Padding = new Padding(5);
            ControlPanel.Height = ZOrderButton.Height + 2 * 5;
            ControlPanel.CreateControl();
            ControlPanel.Controls.Add(ZOrderLabel);
            ControlPanel.Controls.Add(ZOrderButton);
            Win32.SetParent(ControlPanel.Handle, Handle);
            ControlPanel.Visible = true;
        }

        protected override void OnBoundsChangedCore(Rectangle prevBounds, Rectangle newBounds)
        {
            base.OnBoundsChangedCore(prevBounds, newBounds);
            UpdateChildrenBounds();
        }

        protected override bool OnPaintCore(IntPtr hDC)
        {
            FillBackground(hDC, KeyColor);
            return true;
        }

        private void UpdateChildrenBounds()
        {
            var clientRect = ClientRect;
            var controlPanelBounds = new Rectangle(clientRect.X, clientRect.Y, clientRect.Width, ControlPanel.Height);
            ControlPanel.Bounds = controlPanelBounds;
        }

        private void UpdateZOrderText()
        {
            ZOrderLabel.Text = MainWindow.ZOrderText;
        }

        private void OnZOrderButtonClick(object sender, EventArgs e)
        {
            MainWindow.ToggleZOrder();
            UpdateZOrderText();
        }
    }
}
