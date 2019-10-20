using MySql.Data.MySqlClient;
using sampgamemode.Database.Repositories;
using System;

namespace sampgamemode.Database.UnitsOfWork
{
    public class UnitOfWork : IDisposable
    {
        private MySqlConnection _connection;
        private MySqlTransaction _transaction;

        private bool disposed = false;

        private PlayerAccountRepository _playerAccountRepository;
        private PlayerBankAccountRepository _playerBankAccountRepository;

        public UnitOfWork(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public PlayerAccountRepository PlayerAccountRepository => _playerAccountRepository ?? (_playerAccountRepository = new PlayerAccountRepository(_transaction));
        public PlayerBankAccountRepository PlayerBankAccountRepository => _playerBankAccountRepository ?? (_playerBankAccountRepository = new PlayerBankAccountRepository(_transaction));

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }

        public async void CommitAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = await _connection.BeginTransactionAsync();
                ResetRepositories();
            }
        }

        private void ResetRepositories()
        {
            _playerAccountRepository = null;
            _playerBankAccountRepository = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    // Dispose managed resources.
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }

                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }

                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here. 
                // If disposing is false, 
                // only the following code is executed.
                _playerAccountRepository = null;

                // Note disposing has been done.
                disposed = true;

            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
