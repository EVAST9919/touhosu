using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Touhosu.UI.Objects;
using osu.Game.Rulesets.UI;
using osuTK;
using osu.Game.Rulesets.Touhosu.UI.HUD;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osu.Game.Rulesets.Touhosu.Extensions;

namespace osu.Game.Rulesets.Touhosu.UI
{
    public class TouhosuPlayfield : Playfield
    {
        public static readonly Vector2 FULL_SIZE = new Vector2(512, 384);
        public static readonly Vector2 PLAYFIELD_SIZE = new Vector2(307, 384);
        public static readonly float X_SCALE_MULTIPLIER = 0.6f;

        private readonly TouhosuRuleset ruleset;
        public TouhosuPlayer Player;

        public TouhosuPlayfield(TouhosuRuleset ruleset)
        {
            this.ruleset = ruleset;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = PLAYFIELD_SIZE,
                    Masking = true,
                    CornerRadius = 7,
                    Children = new Drawable[]
                    {
                        new PlayfieldBackground(),
                        Player = new TouhosuPlayer(),
                        HitObjectContainer
                    }
                }
            };

            Player.HitObjects = HitObjectContainer;
        }

        public bool CheckHit(DrawableProjectile obj)
        {
            var radius = obj.Size.X / 2;
            var distance = MathExtensions.Distance(Player.PlayerPosition(), obj.Position);
            var isHit = distance < radius + 2;

            if (isHit)
                Player.PlayMissAnimation();

            return isHit;
        }

        public float CheckDistance(DrawableProjectile obj) => (float)MathExtensions.Distance(Player.PlayerPosition(), obj.Position);

        public float GetPlayerAngle(DrawableHomingProjectile obj) => MathExtensions.GetPlayerAngle(Player, obj.Position);

        public override void Add(DrawableHitObject h)
        {
            base.Add(h);

            if (h is DrawableProjectile bullet)
            {
                bullet.CheckHit += CheckHit;
                bullet.CheckDistance += CheckDistance;
            }

            if (h is DrawableHomingProjectile homingBullet)
            {
                homingBullet.PlayerAngle = GetPlayerAngle;
            }
        }

        private bool failInvoked;

        protected override void Update()
        {
            base.Update();

            if (!ruleset.HealthProcessor.HasFailed)
                return;

            if (failInvoked)
                return;

            onFail();
            failInvoked = true;
        }

        private void onFail()
        {
            Player.Die();
        }
    }
}
