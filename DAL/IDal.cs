namespace DAL
{
    public interface IDal<TPrimaryKey, TEntity>
    {
        TEntity FindById(TPrimaryKey id);
        TPrimaryKey Create(TEntity entity);
        
        void Update(TEntity entity);
        void Delete(TPrimaryKey id);
    }
}