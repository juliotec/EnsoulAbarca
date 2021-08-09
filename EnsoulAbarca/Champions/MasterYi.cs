using System.Linq;
using System.Threading.Tasks;
using EnsoulSharp;
using EnsoulSharp.SDK;

namespace EnsoulAbarca.Champions
{
    public class MasterYi
    {
        #region Properties

        public static string Name { get; private set; } = "MasterYi";

        public static SpellDataInst Q { get; private set; } = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q);

        public static SpellDataInst W { get; private set; } = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W);

        public static SpellDataInst E { get; private set; } = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E);

        public static SpellDataInst R { get; private set; } = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R);

        public static bool IsAutoAttack { get; private set; }

        #endregion
        #region Methods

        public static void AutoLevelSpellQWE()
        {
            if (Q.Level == 5 && W.Level == 5 && E.Level == 5 && R.Level == 3)
            {
                return;
            }

            var oldLevel = R.Level;

            ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.R);

            if (R.Level > oldLevel)
            {
                return;
            }
            else if (Q.Level == 0 || (Q.Level < 5 && W.Level > 0 && E.Level > 0 && R.Level > 0))
            {
                ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.Q);
            }
            else if (E.Level == 0 || Q.Level == 5)
            {
                ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.E);
            }
            else
            {
                ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.W);
            }
        }

        public static void AutoAttack()
        {
            if (IsAutoAttack || ObjectManager.Player.IsDead)
            {
                return;
            }

            if (Q.IsReady())
            {
                var target = TargetSelector.GetTargets(Q.SData.CastRadius, DamageType.Mixed, true).OrderBy(i => i.Health).FirstOrDefault();

                if (target != null)
                {
                    IsAutoAttack = true;

                    var flaskSlot = ObjectManager.Player.GetSpellSlot("ItemCrystalFlask");

                    if (flaskSlot != SpellSlot.Unknown)
                    {
                        ObjectManager.Player.Spellbook.CastSpell(flaskSlot);
                    }

                    if (R.IsReady())
                    {
                        ObjectManager.Player.Spellbook.CastSpell(SpellSlot.R);
                    }

                    if (E.IsReady())
                    {
                        ObjectManager.Player.Spellbook.CastSpell(SpellSlot.E);
                    }

                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Q, target);

                    Task.Run(async () =>
                    {
                        await Task.Delay(1000);
                        IsAutoAttack = false;
                    });
                }
            }
        }

        #endregion
    }
}
