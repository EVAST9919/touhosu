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
using osuTK.Graphics;
using osu.Framework.Graphics.Effects;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;

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
                    BorderThickness = 1.2f,
                    BorderColour = Color4.White,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Hollow = true,
                        Radius = 10,
                        Colour = Color4.Black.Opacity(0.4f),
                        Type = EdgeEffectType.Shadow,
                    },
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
            var distance = MathExtensions.Distance(Player.PlayerPosition(), obj.Position + obj.ParentPosition);
            var isHit = distance < radius + 2;

            if (isHit)
                Player.PlayMissAnimation();

            return isHit;
        }

        public float GetDistanceFromPlayer(DrawableProjectile obj) => (float)MathExtensions.Distance(Player.PlayerPosition(), obj.Position + obj.ParentPosition);

        public float GetPlayerAngle(DrawableHomingProjectile obj) => MathExtensions.GetPlayerAngle(Player, obj.Position + obj.ParentPosition);

        public override void Add(DrawableHitObject h)
        {
            base.Add(h);

            if (h is DrawableExplosion explosion)
            {
                explosion.ProjectilesContainer.ForEach(p =>
                {
                    p.CheckHit += CheckHit;
                    p.GetDistanceFromPlayer += GetDistanceFromPlayer;
                });

                return;
            }

            if (h is DrawableSpinner spinner)
            {
                spinner.ProjectilesContainer.ForEach(p =>
                {
                    p.CheckHit += CheckHit;
                    p.GetDistanceFromPlayer += GetDistanceFromPlayer;
                });

                return;
            }

            if (h is DrawableHomingProjectile homing)
            {
                homing.PlayerAngle = GetPlayerAngle;
                homing.CheckHit += CheckHit;
                homing.GetDistanceFromPlayer += GetDistanceFromPlayer;

                return;
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
