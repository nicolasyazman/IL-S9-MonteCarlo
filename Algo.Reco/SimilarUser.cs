
namespace Algo
{
    public struct SimilarUser
    {
        public readonly User User;
        public readonly double Similarity;

        public SimilarUser(User u, double s)
        {
            User = u;
            Similarity = s;
        }
    }
}
