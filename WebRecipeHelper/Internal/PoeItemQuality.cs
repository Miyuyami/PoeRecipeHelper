using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WebRecipeHelper.Pages
{
    public class PoeItemQuality : IEquatable<PoeItemQuality>
    {
        public PoeItem Item { get; }
        public int Quality { get; }

        public PoeItemQuality(PoeItem item, int quality)
        {
            this.Item = item;
            this.Quality = quality;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PoeItemQuality);
        }

        public bool Equals([AllowNull] PoeItemQuality other)
        {
            return other != null &&
                   EqualityComparer<PoeItem>.Default.Equals(this.Item, other.Item);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Item);
        }

        public static bool operator ==(PoeItemQuality left, PoeItemQuality right)
        {
            return EqualityComparer<PoeItemQuality>.Default.Equals(left, right);
        }

        public static bool operator !=(PoeItemQuality left, PoeItemQuality right)
        {
            return !(left == right);
        }
    }
}
