using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Touhosu.UI.Objects;
using osu.Game.Rulesets.UI;
using osuTK;
using osu.Game.Rulesets.Touhosu.UI.HUD;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK.Graphics;
using osu.Framework.Graphics.Effects;
using osu.Framework.Extensions.Color4Extensions;
using osu.Game.Rulesets.Touhosu.Scoring;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osu.Game.Rulesets.Touhosu.Extensions;

namespace osu.Game.Rulesets.Touhosu.UI
{
    public class TouhosuPlayfield : Playfield
    {
        public static readonly Vector2 FULL_SIZE = new Vector2(512, 384);
        public static readonly Vector2 PLAYFIELD_SIZE = new Vector2(307, 384);

        public TouhosuPlayer Player;

        public TouhosuPlayfield()
        {
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
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

        [BackgroundDependencyLoader(true)]
        private void load()
        {
            RegisterPool<AngeledProjectile, DrawableAngeledProjectile>(300, 1500);
            RegisterPool<InstantProjectile, DrawableInstantProjectile>(300, 600);
        }

        protected override void OnNewDrawableHitObject(DrawableHitObject drawableHitObject)
        {
            base.OnNewDrawableHitObject(drawableHitObject);

            switch (drawableHitObject)
            {
                case DrawableAngeledProjectile projectile:
                    projectile.CheckHit += checkHit;
                    projectile.DistanceToPlayer += getDistanceToPlayer;
                    break;
            }
        }

        private bool checkHit(Vector2 pos)
        {
            var isHit = Vector2.Distance(Player.PlayerPosition(), pos) < TouhosuPlayer.HITBOX_SIZE;

            if (isHit)
                Player.PlayMissAnimation();

            return isHit;
        }

        private float getDistanceToPlayer(Vector2 pos) => Vector2.Distance(Player.PlayerPosition(), pos);

        public void ApplyHealthProcessor(TouhosuHealthProcessor p)
        {
            p.Failed += onFail;
        }

        private bool onFail()
        {
            Player.Die();
            return true;
        }

        protected override HitObjectLifetimeEntry CreateLifetimeEntry(HitObject hitObject) => new TouhosuHitObjectLifetimeEntry(hitObject);

        private class TouhosuHitObjectLifetimeEntry : HitObjectLifetimeEntry
        {
            public TouhosuHitObjectLifetimeEntry(HitObject hitObject)
                : base(hitObject)
            {
            }

            protected override double InitialLifetimeOffset
            {
                get
                {
                    if (HitObject is Projectile projectile)
                        return projectile.TimePreempt;

                    return base.InitialLifetimeOffset;
                }
            }
        }
    }
}
