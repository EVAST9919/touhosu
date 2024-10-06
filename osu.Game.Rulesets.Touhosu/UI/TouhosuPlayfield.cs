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
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics.Shapes;

namespace osu.Game.Rulesets.Touhosu.UI
{
    public partial class TouhosuPlayfield : Playfield
    {
        public static readonly Vector2 FULL_SIZE = new Vector2(512, 384);
        public static readonly Vector2 PLAYFIELD_SIZE = new Vector2(307, 384);

        public double SpeedMultiplier
        {
            get => Player.SpeedMultiplier;
            set => Player.SpeedMultiplier = value;
        }

        private Sample grazeSample;

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
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Hollow = true,
                        Radius = 10,
                        Colour = Color4.Black.Opacity(0.4f),
                        Type = EdgeEffectType.Shadow,
                    },
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0,
                        AlwaysPresent = true
                    }
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(1),
                    Child = new Container
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Masking = true,
                        CornerRadius = 7,
                        Children = new Drawable[]
                        {
                            new PlayfieldBackground(),
                            Player = new TouhosuPlayer(),
                            HitObjectContainer
                        }
                    }
                },
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    CornerRadius = 7,
                    BorderThickness = 1.2f,
                    BorderColour = Color4.White,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0,
                        AlwaysPresent = true
                    }
                },
            };

            Player.HitObjects = HitObjectContainer;
        }

        [BackgroundDependencyLoader(true)]
        private void load(ISampleStore samples)
        {
            RegisterPool<AngeledProjectile, DrawableAngeledProjectile>(300, 1500);
            RegisterPool<InstantProjectile, DrawableInstantProjectile>(300, 600);
            grazeSample = samples.Get("graze");
        }

        protected override void OnNewDrawableHitObject(DrawableHitObject drawableHitObject)
        {
            base.OnNewDrawableHitObject(drawableHitObject);

            switch (drawableHitObject)
            {
                case DrawableAngeledProjectile projectile:
                    projectile.CheckHit += checkHit;
                    projectile.DistanceToPlayer += getDistanceToPlayer;
                    projectile.CheckGrazed += checkGrazed;
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

        private bool checkGrazed(Vector2 pos)
        {
            var isGrazed = Vector2.Distance(Player.PlayerPosition(), pos) < TouhosuPlayer.GRAZE_SIZE;

            if (isGrazed)
            {
                grazeSample?.Play();
            }

            return isGrazed;
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
