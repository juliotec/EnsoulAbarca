using System;
using System.Drawing;
using EnsoulSharp;
using EnsoulSharp.SDK.MenuUI;
using EnsoulAbarca.Champions;


namespace EnsoulAbarca
{
    public static class MainCheat
    {
        #region Properties

        public static string Id { get; private set; } = "EnsoulAbarca";
        public static string SkinsId { get; private set; } = $@"{Id}Skins";
        public static string IsSkinChangerId { get; private set; } = $@"{Id}IsSkinChanger";

        public static Menu MainMenu
        {
            get
            {
                if (_mainMenu == null)
                {
                    _mainMenu = new Menu(Id, Id, true);
                    _mainMenu.Attach();
                    _mainMenu.Add(IsSkinChangerMenu);
                    _mainMenu.Add(SkinMenu);
                }

                return _mainMenu;
            }
        }
        private static Menu _mainMenu;
        
        public static MenuSlider SkinMenu
        {
            get
            {
                if (_skinMenu == null)
                {
                    _skinMenu = new MenuSlider(SkinsId, "Skins", 0, 0, 100);
                    _skinMenu.ValueChanged += SkinListOnValueChanged;
                }
                
                return _skinMenu;
            }
        }
        private static MenuSlider _skinMenu;

        public static MenuBool IsSkinChangerMenu { get; private set; } = new MenuBool(IsSkinChangerId, "Use Skin Changer?", false);

        #endregion
        #region Methods

        public static void OnGameLoad()
        {
            Game.OnNotify += OnNotify;
            Game.Print($@"{Id} loaded", Color.Coral);
            ObjectManager.Player.SetSkin(MainMenu.GetValue<MenuSlider>().Value);

        }

        private static void SetSkinId()
        {
            if (!MainMenu.GetValue<MenuBool>().Enabled)
            {
                return;
            }

            ObjectManager.Player.SetSkin(MainMenu.GetValue<MenuSlider>().Value);
        }

        private static void OnNotify(GameNotifyEventArgs args)
        {
            switch (args.EventId)
            {
                case GameEventId.OnReincarnate:
                case GameEventId.OnResetChampion:
                    SetSkinId();
                    break;
            }
        }

        private static void SkinListOnValueChanged(object sender, EventArgs e) => SetSkinId();

        #endregion
    }
}
