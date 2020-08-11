using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;
using System.Linq;

namespace osu.Game.Rulesets.Touhosu.Edit
{
    public class TouhosuSelectionHandler : SelectionHandler
    {
        public override bool HandleMovement(MoveSelectionEvent moveEvent)
        {
            Vector2 minPosition = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 maxPosition = new Vector2(float.MinValue, float.MinValue);

            // Go through all hitobjects to make sure they would remain in the bounds of the editor after movement, before any movement is attempted
            foreach (var h in SelectedHitObjects.OfType<TouhosuHitObject>())
            {
                minPosition = Vector2.ComponentMin(minPosition, h.Position + moveEvent.InstantDelta);
                maxPosition = Vector2.ComponentMax(maxPosition, h.Position + moveEvent.InstantDelta);
            }

            if (minPosition.X < 0 || minPosition.Y < 0 || maxPosition.X > DrawWidth || maxPosition.Y > DrawHeight)
                return false;

            foreach (var h in SelectedHitObjects.OfType<TouhosuHitObject>())
            {
                h.Position += moveEvent.InstantDelta;
            }

            return true;
        }
    }
}
