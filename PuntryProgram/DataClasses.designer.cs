﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PuntryProgram
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="PuntryProgram")]
	public partial class DataClassesDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Определения метода расширяемости
    partial void OnCreated();
    partial void InsertFavoriteFiles(FavoriteFiles instance);
    partial void UpdateFavoriteFiles(FavoriteFiles instance);
    partial void DeleteFavoriteFiles(FavoriteFiles instance);
    partial void InsertUsers(Users instance);
    partial void UpdateUsers(Users instance);
    partial void DeleteUsers(Users instance);
    partial void InsertFileChanges(FileChanges instance);
    partial void UpdateFileChanges(FileChanges instance);
    partial void DeleteFileChanges(FileChanges instance);
    partial void InsertFiles(Files instance);
    partial void UpdateFiles(Files instance);
    partial void DeleteFiles(Files instance);
    #endregion
		
		public DataClassesDataContext() : 
				base(global::PuntryProgram.Properties.Settings.Default.PuntryProgramConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DataClassesDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClassesDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClassesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClassesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<FavoriteFiles> FavoriteFiles
		{
			get
			{
				return this.GetTable<FavoriteFiles>();
			}
		}
		
		public System.Data.Linq.Table<Users> Users
		{
			get
			{
				return this.GetTable<Users>();
			}
		}
		
		public System.Data.Linq.Table<FileChanges> FileChanges
		{
			get
			{
				return this.GetTable<FileChanges>();
			}
		}
		
		public System.Data.Linq.Table<Files> Files
		{
			get
			{
				return this.GetTable<Files>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.FavoriteFiles")]
	public partial class FavoriteFiles : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private System.Nullable<int> _user_id;
		
		private System.Nullable<int> _file_id;
		
		private EntityRef<Users> _Users;
		
		private EntityRef<Files> _Files;
		
    #region Определения метода расширяемости
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void Onuser_idChanging(System.Nullable<int> value);
    partial void Onuser_idChanged();
    partial void Onfile_idChanging(System.Nullable<int> value);
    partial void Onfile_idChanged();
    #endregion
		
		public FavoriteFiles()
		{
			this._Users = default(EntityRef<Users>);
			this._Files = default(EntityRef<Files>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_user_id", DbType="Int")]
		public System.Nullable<int> user_id
		{
			get
			{
				return this._user_id;
			}
			set
			{
				if ((this._user_id != value))
				{
					if (this._Users.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.Onuser_idChanging(value);
					this.SendPropertyChanging();
					this._user_id = value;
					this.SendPropertyChanged("user_id");
					this.Onuser_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_file_id", DbType="Int")]
		public System.Nullable<int> file_id
		{
			get
			{
				return this._file_id;
			}
			set
			{
				if ((this._file_id != value))
				{
					if (this._Files.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.Onfile_idChanging(value);
					this.SendPropertyChanging();
					this._file_id = value;
					this.SendPropertyChanged("file_id");
					this.Onfile_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Users_FavoriteFiles", Storage="_Users", ThisKey="user_id", OtherKey="id", IsForeignKey=true)]
		public Users Users
		{
			get
			{
				return this._Users.Entity;
			}
			set
			{
				Users previousValue = this._Users.Entity;
				if (((previousValue != value) 
							|| (this._Users.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Users.Entity = null;
						previousValue.FavoriteFiles.Remove(this);
					}
					this._Users.Entity = value;
					if ((value != null))
					{
						value.FavoriteFiles.Add(this);
						this._user_id = value.id;
					}
					else
					{
						this._user_id = default(Nullable<int>);
					}
					this.SendPropertyChanged("Users");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Files_FavoriteFiles", Storage="_Files", ThisKey="file_id", OtherKey="id", IsForeignKey=true)]
		public Files Files
		{
			get
			{
				return this._Files.Entity;
			}
			set
			{
				Files previousValue = this._Files.Entity;
				if (((previousValue != value) 
							|| (this._Files.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Files.Entity = null;
						previousValue.FavoriteFiles.Remove(this);
					}
					this._Files.Entity = value;
					if ((value != null))
					{
						value.FavoriteFiles.Add(this);
						this._file_id = value.id;
					}
					else
					{
						this._file_id = default(Nullable<int>);
					}
					this.SendPropertyChanged("Files");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Users")]
	public partial class Users : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private string _name;
		
		private string _surname;
		
		private string _login;
		
		private System.Nullable<System.Guid> _password;
		
		private System.Data.Linq.Binary _image;
		
		private System.Nullable<System.DateTime> _datetime_at;
		
		private bool _admin;
		
		private bool _editor;
		
		private EntitySet<FavoriteFiles> _FavoriteFiles;
		
		private EntitySet<FileChanges> _FileChanges;
		
    #region Определения метода расширяемости
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void OnnameChanging(string value);
    partial void OnnameChanged();
    partial void OnsurnameChanging(string value);
    partial void OnsurnameChanged();
    partial void OnloginChanging(string value);
    partial void OnloginChanged();
    partial void OnpasswordChanging(System.Nullable<System.Guid> value);
    partial void OnpasswordChanged();
    partial void OnimageChanging(System.Data.Linq.Binary value);
    partial void OnimageChanged();
    partial void Ondatetime_atChanging(System.Nullable<System.DateTime> value);
    partial void Ondatetime_atChanged();
    partial void OnadminChanging(bool value);
    partial void OnadminChanged();
    partial void OneditorChanging(bool value);
    partial void OneditorChanged();
    #endregion
		
		public Users()
		{
			this._FavoriteFiles = new EntitySet<FavoriteFiles>(new Action<FavoriteFiles>(this.attach_FavoriteFiles), new Action<FavoriteFiles>(this.detach_FavoriteFiles));
			this._FileChanges = new EntitySet<FileChanges>(new Action<FileChanges>(this.attach_FileChanges), new Action<FileChanges>(this.detach_FileChanges));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_name", DbType="NVarChar(50)")]
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				if ((this._name != value))
				{
					this.OnnameChanging(value);
					this.SendPropertyChanging();
					this._name = value;
					this.SendPropertyChanged("name");
					this.OnnameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_surname", DbType="NVarChar(50)")]
		public string surname
		{
			get
			{
				return this._surname;
			}
			set
			{
				if ((this._surname != value))
				{
					this.OnsurnameChanging(value);
					this.SendPropertyChanging();
					this._surname = value;
					this.SendPropertyChanged("surname");
					this.OnsurnameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_login", DbType="NVarChar(50)")]
		public string login
		{
			get
			{
				return this._login;
			}
			set
			{
				if ((this._login != value))
				{
					this.OnloginChanging(value);
					this.SendPropertyChanging();
					this._login = value;
					this.SendPropertyChanged("login");
					this.OnloginChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_password", DbType="UniqueIdentifier")]
		public System.Nullable<System.Guid> password
		{
			get
			{
				return this._password;
			}
			set
			{
				if ((this._password != value))
				{
					this.OnpasswordChanging(value);
					this.SendPropertyChanging();
					this._password = value;
					this.SendPropertyChanged("password");
					this.OnpasswordChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_image", DbType="VarBinary(MAX)", UpdateCheck=UpdateCheck.Never)]
		public System.Data.Linq.Binary image
		{
			get
			{
				return this._image;
			}
			set
			{
				if ((this._image != value))
				{
					this.OnimageChanging(value);
					this.SendPropertyChanging();
					this._image = value;
					this.SendPropertyChanged("image");
					this.OnimageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_datetime_at", DbType="DateTime")]
		public System.Nullable<System.DateTime> datetime_at
		{
			get
			{
				return this._datetime_at;
			}
			set
			{
				if ((this._datetime_at != value))
				{
					this.Ondatetime_atChanging(value);
					this.SendPropertyChanging();
					this._datetime_at = value;
					this.SendPropertyChanged("datetime_at");
					this.Ondatetime_atChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_admin", DbType="Bit NOT NULL")]
		public bool admin
		{
			get
			{
				return this._admin;
			}
			set
			{
				if ((this._admin != value))
				{
					this.OnadminChanging(value);
					this.SendPropertyChanging();
					this._admin = value;
					this.SendPropertyChanged("admin");
					this.OnadminChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_editor", DbType="Bit NOT NULL")]
		public bool editor
		{
			get
			{
				return this._editor;
			}
			set
			{
				if ((this._editor != value))
				{
					this.OneditorChanging(value);
					this.SendPropertyChanging();
					this._editor = value;
					this.SendPropertyChanged("editor");
					this.OneditorChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Users_FavoriteFiles", Storage="_FavoriteFiles", ThisKey="id", OtherKey="user_id")]
		public EntitySet<FavoriteFiles> FavoriteFiles
		{
			get
			{
				return this._FavoriteFiles;
			}
			set
			{
				this._FavoriteFiles.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Users_FileChanges", Storage="_FileChanges", ThisKey="id", OtherKey="user_id")]
		public EntitySet<FileChanges> FileChanges
		{
			get
			{
				return this._FileChanges;
			}
			set
			{
				this._FileChanges.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_FavoriteFiles(FavoriteFiles entity)
		{
			this.SendPropertyChanging();
			entity.Users = this;
		}
		
		private void detach_FavoriteFiles(FavoriteFiles entity)
		{
			this.SendPropertyChanging();
			entity.Users = null;
		}
		
		private void attach_FileChanges(FileChanges entity)
		{
			this.SendPropertyChanging();
			entity.Users = this;
		}
		
		private void detach_FileChanges(FileChanges entity)
		{
			this.SendPropertyChanging();
			entity.Users = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.FileChanges")]
	public partial class FileChanges : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private string _comment;
		
		private System.Nullable<System.DateTime> _datetime_up;
		
		private System.Nullable<int> _user_id;
		
		private System.Nullable<int> _file_id;
		
		private EntityRef<Users> _Users;
		
		private EntityRef<Files> _Files;
		
    #region Определения метода расширяемости
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void OncommentChanging(string value);
    partial void OncommentChanged();
    partial void Ondatetime_upChanging(System.Nullable<System.DateTime> value);
    partial void Ondatetime_upChanged();
    partial void Onuser_idChanging(System.Nullable<int> value);
    partial void Onuser_idChanged();
    partial void Onfile_idChanging(System.Nullable<int> value);
    partial void Onfile_idChanged();
    #endregion
		
		public FileChanges()
		{
			this._Users = default(EntityRef<Users>);
			this._Files = default(EntityRef<Files>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_comment", DbType="NVarChar(250)")]
		public string comment
		{
			get
			{
				return this._comment;
			}
			set
			{
				if ((this._comment != value))
				{
					this.OncommentChanging(value);
					this.SendPropertyChanging();
					this._comment = value;
					this.SendPropertyChanged("comment");
					this.OncommentChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_datetime_up", DbType="DateTime")]
		public System.Nullable<System.DateTime> datetime_up
		{
			get
			{
				return this._datetime_up;
			}
			set
			{
				if ((this._datetime_up != value))
				{
					this.Ondatetime_upChanging(value);
					this.SendPropertyChanging();
					this._datetime_up = value;
					this.SendPropertyChanged("datetime_up");
					this.Ondatetime_upChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_user_id", DbType="Int")]
		public System.Nullable<int> user_id
		{
			get
			{
				return this._user_id;
			}
			set
			{
				if ((this._user_id != value))
				{
					if (this._Users.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.Onuser_idChanging(value);
					this.SendPropertyChanging();
					this._user_id = value;
					this.SendPropertyChanged("user_id");
					this.Onuser_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_file_id", DbType="Int")]
		public System.Nullable<int> file_id
		{
			get
			{
				return this._file_id;
			}
			set
			{
				if ((this._file_id != value))
				{
					if (this._Files.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.Onfile_idChanging(value);
					this.SendPropertyChanging();
					this._file_id = value;
					this.SendPropertyChanged("file_id");
					this.Onfile_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Users_FileChanges", Storage="_Users", ThisKey="user_id", OtherKey="id", IsForeignKey=true)]
		public Users Users
		{
			get
			{
				return this._Users.Entity;
			}
			set
			{
				Users previousValue = this._Users.Entity;
				if (((previousValue != value) 
							|| (this._Users.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Users.Entity = null;
						previousValue.FileChanges.Remove(this);
					}
					this._Users.Entity = value;
					if ((value != null))
					{
						value.FileChanges.Add(this);
						this._user_id = value.id;
					}
					else
					{
						this._user_id = default(Nullable<int>);
					}
					this.SendPropertyChanged("Users");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Files_FileChanges", Storage="_Files", ThisKey="file_id", OtherKey="id", IsForeignKey=true)]
		public Files Files
		{
			get
			{
				return this._Files.Entity;
			}
			set
			{
				Files previousValue = this._Files.Entity;
				if (((previousValue != value) 
							|| (this._Files.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Files.Entity = null;
						previousValue.FileChanges.Remove(this);
					}
					this._Files.Entity = value;
					if ((value != null))
					{
						value.FileChanges.Add(this);
						this._file_id = value.id;
					}
					else
					{
						this._file_id = default(Nullable<int>);
					}
					this.SendPropertyChanged("Files");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Files")]
	public partial class Files : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private string _name;
		
		private string _comment;
		
		private System.Data.Linq.Binary _binary;
		
		private string _extension;
		
		private System.Nullable<int> _size;
		
		private bool _project;
		
		private bool _review;
		
		private bool _archive;
		
		private System.Nullable<int> _user_id;
		
		private EntitySet<FavoriteFiles> _FavoriteFiles;
		
		private EntitySet<FileChanges> _FileChanges;
		
    #region Определения метода расширяемости
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void OnnameChanging(string value);
    partial void OnnameChanged();
    partial void OncommentChanging(string value);
    partial void OncommentChanged();
    partial void OnbinaryChanging(System.Data.Linq.Binary value);
    partial void OnbinaryChanged();
    partial void OnextensionChanging(string value);
    partial void OnextensionChanged();
    partial void OnsizeChanging(System.Nullable<int> value);
    partial void OnsizeChanged();
    partial void OnprojectChanging(bool value);
    partial void OnprojectChanged();
    partial void OnreviewChanging(bool value);
    partial void OnreviewChanged();
    partial void OnarchiveChanging(bool value);
    partial void OnarchiveChanged();
    partial void Onuser_idChanging(System.Nullable<int> value);
    partial void Onuser_idChanged();
    #endregion
		
		public Files()
		{
			this._FavoriteFiles = new EntitySet<FavoriteFiles>(new Action<FavoriteFiles>(this.attach_FavoriteFiles), new Action<FavoriteFiles>(this.detach_FavoriteFiles));
			this._FileChanges = new EntitySet<FileChanges>(new Action<FileChanges>(this.attach_FileChanges), new Action<FileChanges>(this.detach_FileChanges));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_name", DbType="NVarChar(50)")]
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				if ((this._name != value))
				{
					this.OnnameChanging(value);
					this.SendPropertyChanging();
					this._name = value;
					this.SendPropertyChanged("name");
					this.OnnameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_comment", DbType="NVarChar(250)")]
		public string comment
		{
			get
			{
				return this._comment;
			}
			set
			{
				if ((this._comment != value))
				{
					this.OncommentChanging(value);
					this.SendPropertyChanging();
					this._comment = value;
					this.SendPropertyChanged("comment");
					this.OncommentChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_binary", DbType="VarBinary(MAX)", UpdateCheck=UpdateCheck.Never)]
		public System.Data.Linq.Binary binary
		{
			get
			{
				return this._binary;
			}
			set
			{
				if ((this._binary != value))
				{
					this.OnbinaryChanging(value);
					this.SendPropertyChanging();
					this._binary = value;
					this.SendPropertyChanged("binary");
					this.OnbinaryChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_extension", DbType="NVarChar(20)")]
		public string extension
		{
			get
			{
				return this._extension;
			}
			set
			{
				if ((this._extension != value))
				{
					this.OnextensionChanging(value);
					this.SendPropertyChanging();
					this._extension = value;
					this.SendPropertyChanged("extension");
					this.OnextensionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_size", DbType="Int")]
		public System.Nullable<int> size
		{
			get
			{
				return this._size;
			}
			set
			{
				if ((this._size != value))
				{
					this.OnsizeChanging(value);
					this.SendPropertyChanging();
					this._size = value;
					this.SendPropertyChanged("size");
					this.OnsizeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_project", DbType="Bit NOT NULL")]
		public bool project
		{
			get
			{
				return this._project;
			}
			set
			{
				if ((this._project != value))
				{
					this.OnprojectChanging(value);
					this.SendPropertyChanging();
					this._project = value;
					this.SendPropertyChanged("project");
					this.OnprojectChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_review", DbType="Bit NOT NULL")]
		public bool review
		{
			get
			{
				return this._review;
			}
			set
			{
				if ((this._review != value))
				{
					this.OnreviewChanging(value);
					this.SendPropertyChanging();
					this._review = value;
					this.SendPropertyChanged("review");
					this.OnreviewChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_archive", DbType="Bit NOT NULL")]
		public bool archive
		{
			get
			{
				return this._archive;
			}
			set
			{
				if ((this._archive != value))
				{
					this.OnarchiveChanging(value);
					this.SendPropertyChanging();
					this._archive = value;
					this.SendPropertyChanged("archive");
					this.OnarchiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_user_id", DbType="Int")]
		public System.Nullable<int> user_id
		{
			get
			{
				return this._user_id;
			}
			set
			{
				if ((this._user_id != value))
				{
					this.Onuser_idChanging(value);
					this.SendPropertyChanging();
					this._user_id = value;
					this.SendPropertyChanged("user_id");
					this.Onuser_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Files_FavoriteFiles", Storage="_FavoriteFiles", ThisKey="id", OtherKey="file_id")]
		public EntitySet<FavoriteFiles> FavoriteFiles
		{
			get
			{
				return this._FavoriteFiles;
			}
			set
			{
				this._FavoriteFiles.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Files_FileChanges", Storage="_FileChanges", ThisKey="id", OtherKey="file_id")]
		public EntitySet<FileChanges> FileChanges
		{
			get
			{
				return this._FileChanges;
			}
			set
			{
				this._FileChanges.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_FavoriteFiles(FavoriteFiles entity)
		{
			this.SendPropertyChanging();
			entity.Files = this;
		}
		
		private void detach_FavoriteFiles(FavoriteFiles entity)
		{
			this.SendPropertyChanging();
			entity.Files = null;
		}
		
		private void attach_FileChanges(FileChanges entity)
		{
			this.SendPropertyChanging();
			entity.Files = this;
		}
		
		private void detach_FileChanges(FileChanges entity)
		{
			this.SendPropertyChanging();
			entity.Files = null;
		}
	}
}
#pragma warning restore 1591
