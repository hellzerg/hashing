namespace Hashing
{
    internal sealed class DuplicateHashMatch
    {
        internal readonly HashCode Algorithm;
        internal readonly string AlgorithmName;
        internal readonly string Hash;

        internal DuplicateHashMatch(HashCode algorithm, string algorithmName, string hash)
        {
            Algorithm = algorithm;
            AlgorithmName = algorithmName;
            Hash = hash;
        }
    }

    internal sealed class DuplicateGroupInfo
    {
        internal readonly HashCode Algorithm;
        internal readonly string AlgorithmName;
        internal readonly string Hash;
        internal readonly SumResult[] Items;
        internal readonly System.Collections.Generic.List<DuplicateHashMatch> HashMatches;

        internal DuplicateGroupInfo(HashCode algorithm, string algorithmName, string hash, SumResult[] items)
        {
            Algorithm = algorithm;
            AlgorithmName = algorithmName;
            Hash = hash;
            Items = items;
            HashMatches = new System.Collections.Generic.List<DuplicateHashMatch>(2)
            {
                new DuplicateHashMatch(algorithm, algorithmName, hash)
            };
        }

        internal void AddHashMatch(HashCode algorithm, string algorithmName, string hash)
        {
            foreach (DuplicateHashMatch match in HashMatches)
            {
                if (match.Algorithm == algorithm && string.Equals(match.Hash, hash, System.StringComparison.OrdinalIgnoreCase))
                    return;
            }

            HashMatches.Add(new DuplicateHashMatch(algorithm, algorithmName, hash));
        }
    }
}
