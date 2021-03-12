namespace Subway.Status.Domain.Dtos
{
    public class Line : IIdentificable
    {
        public string Id { get; set; }
        //public int DirectionId { get; set; }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Line line = (Line)obj;
                return Id == line.Id;
            }
        }
    }
}
