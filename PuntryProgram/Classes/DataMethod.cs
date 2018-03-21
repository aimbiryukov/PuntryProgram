using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Text.RegularExpressions;

namespace PuntryProgram.Classes
{
    public enum LevelAccess
    {
        Admin,
        Editor,
        User
    }

    public enum StatusFile
    {
        My,
        All,
        Project,
        Review,
        Favorite,
        Archive,
        Draft
    }

    public struct FileStruct
    {
        public int fileId;
        public string name;
        public string comment;
        public string extension;
        public int size;
        public Binary binary;
        public DateTime dateTimeAT;
        public DateTime dateTimeUP;
        public bool reveiw;
        public bool project;
        public bool archive;
        public int userId;

        public FileStruct(int fileId, string name, string comment, string extension, int size, Binary binary, DateTime dateTimeAT, DateTime dateTimeUP, bool reveiw, bool project, bool archive, int userId)
        {
            this.fileId = fileId;
            this.name = name;
            this.comment = comment;
            this.extension = extension;
            this.size = size;
            this.binary = binary;
            this.dateTimeAT = dateTimeAT;
            this.dateTimeUP = dateTimeUP;
            this.reveiw = reveiw;
            this.project = project;
            this.archive = archive;
            this.userId = userId;
        }
    }

    public struct UserStruct
    {
        public int userId;
        public string name;
        public string surname;
        public string login;
        public Guid password;
        public DateTime dateTimeAT;
        public Binary binary;
        public LevelAccess level;

        public UserStruct(int userId, string name, string surname, string login, Guid password, DateTime dateTimeAT, Binary binary, LevelAccess level)
        {
            this.userId = userId;
            this.name = name;
            this.surname = surname;
            this.login = login;
            this.password = password;
            this.dateTimeAT = dateTimeAT;
            this.binary = binary;
            this.level = level;
        }
    }

    public static class DataMethod
    {
        private static DataClassesDataContext _db = new DataClassesDataContext();
        
        public static Regex incorrectChar = new Regex(@"[\\\/\:\?\""\<\>\\|]+");
        public static Regex incorrectCharFullName = new Regex(@"[^0-9a-zA-Zа-яА-Я]+");

        public static Guid GetHash(string password)
        {
            using (var hash = System.Security.Cryptography.MD5.Create())
            {
                return new Guid(string.Concat(hash.ComputeHash(Encoding.Unicode.GetBytes(password)).Select(s => s.ToString("x2"))));
            }
        }

        public static void DisposeDB()
        {
            _db.Dispose();
        }

        public static bool CheckUser(string login, Guid password)
        {
            return _db.Users.Where(w => w.login == login && w.password == password).Any();
        }

        public static bool CheckLogin(string login)
        {
            return _db.Users.Where(w => w.login == login).Any();
        }

        public static IQueryable UsersTable()
        {
            return _db.Users
            .OrderByDescending(o => o.datetime_at)
            .Select(s => new
            {
                id = s.id,
                Логин = s.login,
                Имя = s.name,
                Фамилия = s.surname,
                Статус = (s.admin == true) ? "Администратор" : (s.editor == true) ? "Редактор" : "Пользователь",
                datetime_at = s.datetime_at
            });
        }

        public static IQueryable SearchUsers(string text)
        {
            return _db.Users
                .OrderByDescending(o => o.datetime_at)
                .Select(s => new
                {
                    id = s.id,
                    Логин = s.login,
                    Имя = s.name,
                    Фамилия = s.surname,
                    Статус = (s.admin == true) ? "Администратор" : (s.editor == true) ? "Редактор" : "Пользователь",
                    datetime_at = s.datetime_at
                })
                .Where(w =>
                w.Имя.StartsWith(text) ||
                w.Фамилия.StartsWith(text) ||
                w.Логин.StartsWith(text) ||
                w.Статус.StartsWith(text));
        }

        public static UserStruct GetUser(int userId)
        {
            return _db.Users
                .Where(w => w.id == userId)
                .Select(s => new UserStruct
                (
                    s.id,
                    s.name,
                    s.surname,
                    s.login,
                    (Guid)s.password,
                    (DateTime)s.datetime_at,
                    s.image,
                    (s.admin == true) ? LevelAccess.Admin : (s.editor == true) ? LevelAccess.Editor : LevelAccess.User)
                ).Single();
        }

        public static int GetUserId(string login)
        {
            return _db.Users.Where(w => w.login == login).Select(s => s.id).Single();
        }

        public static void InsertUser(string name, string surname, string login, Guid password, DateTime dateTimeAT, Binary binary, LevelAccess level)
        {
            var newUser = new Users
            {
                name = name,
                surname = surname,
                login = login,
                password = password,
                datetime_at = dateTimeAT,
                image = binary,
                admin = (level == LevelAccess.Admin) ? true : false,
                editor = (level == LevelAccess.Editor) ? true : false,
            };

            _db.Users.InsertOnSubmit(newUser);
            _db.SubmitChanges();
        }

        public static void DeleteUser(int userId)
        {
            var changes = _db.FileChanges.Where(w => w.user_id == userId);
            var favorites = _db.FavoriteFiles.Where(w => w.user_id == userId);
            var files = _db.Files.Where(w => w.user_id == userId);
            var user = _db.Users.Where(w => w.id == userId).Single();

            _db.FileChanges.DeleteAllOnSubmit(changes);
            _db.FavoriteFiles.DeleteAllOnSubmit(favorites);
            _db.Files.DeleteAllOnSubmit(files);
            _db.Users.DeleteOnSubmit(user);
            _db.SubmitChanges();
        }

        public static void UpdateUser(int userId, string name, string surname, string login, Guid password, Binary binary, LevelAccess level)
        {
            var user = _db.Users.Where(u => u.id == userId).Single();

            user.name = name;
            user.surname = surname;
            user.login = login;
            user.password = password;
            user.image = binary;
            user.admin = (level == LevelAccess.Admin) ? true : false;
            user.editor = (level == LevelAccess.Editor) ? true : false;

            _db.SubmitChanges();
        }

        public static FileStruct GetFile(int fileId)
        {
            var dateTimeAT = (DateTime)_db.FileChanges.Where(w => w.file_id == fileId).ToList().First().datetime_up;
            var dateTimeUP = (DateTime)_db.FileChanges.Where(w => w.file_id == fileId).ToList().Last().datetime_up;

            return _db.Files
                .Join(_db.FileChanges, f => f.id, fc => fc.file_id, (f, fc) => new { f, fc })
                .Where(f => f.f.id == fileId)
                .Select(s => new FileStruct
                (
                    s.f.id,
                    s.f.name,
                    s.f.comment,
                    s.f.extension,
                    (int)s.f.size,
                    s.f.binary,
                    dateTimeAT,
                    dateTimeUP,
                    s.f.review,
                    s.f.project,
                    s.f.archive,
                    (int)s.f.user_id)
                ).First();
        }

        public static StatusFile CheckStatusFile(int fileId)
        {
            var review = _db.Files.Where(f => f.id == fileId && f.review == true && f.project == false).Any();
            var global = _db.Files.Where(f => f.id == fileId && f.review == false && f.project == true).Any();

            return (global == true) ? StatusFile.Project : (review == true) ? StatusFile.Review : StatusFile.Draft;
        }

        public static int CountFiles(int userId, StatusFile statusFile)
        {
            return _db.Files
                .Count(c =>
                (statusFile == StatusFile.Draft) ? c.user_id == userId && c.project == false :
                (statusFile == StatusFile.Project) ? c.user_id == userId && c.review == false && c.project == true :
                c.user_id == userId);
        }

        public static int SizeAllFiles(int userId)
        {
            var files = _db.Files.Where(w => w.user_id == userId);
            var result = 0;

            foreach (var file in files)
                result += (int)file.size;

            return result;
        }

        public static IQueryable FilesTable(int userId, StatusFile statusFile)
        {
            return _db.Files
                .Join(_db.Users, f => f.user_id, u => u.id, (f, u) => new { f, u })
                .Where(w =>
                (statusFile == StatusFile.All) ? w.f.archive == false :
                (statusFile == StatusFile.Archive) ? w.u.id == userId && w.f.archive == true :
                (statusFile == StatusFile.Project) ? w.f.project == true && w.f.archive == false :
                (statusFile == StatusFile.Review) ? w.f.review == true && w.f.archive == false :
                w.u.id == userId && w.f.archive == false)
                .Select(s => new
                {
                    id = s.f.id,
                    Название = s.f.name,
                    Описание = s.f.comment,
                    Владелец = s.u.login,
                    Статус = (s.f.project == true) ? "Проект" : (s.f.review == true) ? "На проверке" : "Черновик",
                });
        }

        public static IQueryable FavoritesTable(int userId)
        {
            return _db.FavoriteFiles
                .Join(_db.Users, favorite1 => favorite1.user_id, u => u.id, (favorite1, u) => new { favorite1, u })
                .Join(_db.Files, favorite2 => favorite2.favorite1.file_id, f => f.id, (favorite2, f) => new { favorite2, f })
                .Where(w => w.favorite2.u.id == userId)
                .Select(s => new
                {
                    id = s.f.id,
                    Название = s.f.name,
                    Описание = s.f.comment,
                    Владелец = s.favorite2.u.login,
                    Статус = (s.f.project == true) ? "Проект" : (s.f.review == true) ? "На проверке" : "Черновик",
                });
        }

        public static IQueryable SearchFiles(int userId, string text, StatusFile statusFile)
        {
            return _db.Files
                .Join(_db.Users, f => f.user_id, u => u.id, (f, u) => new { f, u })
                .Where(w =>
                (statusFile == StatusFile.All) ? w.f.archive == false :
                (statusFile == StatusFile.Archive) ? w.u.id == userId && w.f.archive == true :
                (statusFile == StatusFile.Project) ? w.f.project == true && w.f.archive == false :
                (statusFile == StatusFile.Review) ? w.f.review == true && w.f.archive == false :
                w.u.id == userId && w.f.archive == false)
                .Select(s => new
                {
                    id = s.f.id,
                    Название = s.f.name,
                    Описание = s.f.comment,
                    Владелец = s.u.login,
                    Статус = (s.f.project == true) ? "Проект" : (s.f.review == true) ? "На проверке" : "Черновик",
                })
                .Where(w =>
                w.Статус.StartsWith(text) ||
                w.Название.StartsWith(text) ||
                w.Описание.StartsWith(text) ||
                w.Владелец.StartsWith(text));
        }

        public static IQueryable SearchFavorites(int userId, string text)
        {
            return _db.FavoriteFiles
                .Join(_db.Users, favorite1 => favorite1.user_id, u => u.id, (favorite1, u) => new { favorite1, u })
                .Join(_db.Files, favorite2 => favorite2.favorite1.file_id, f => f.id, (favorite2, f) => new { favorite2, f })
                .Where(w => w.favorite2.u.id == userId)
                .Select(s => new
                {
                    id = s.f.id,
                    Название = s.f.name,
                    Описание = s.f.comment,
                    Владелец = s.favorite2.u.login,
                    Статус = (s.f.project == true) ? "Проект" : (s.f.review == true) ? "На проверке" : "Черновик",
                })
                .Where(w =>
                w.Статус.StartsWith(text) ||
                w.Название.StartsWith(text) ||
                w.Описание.StartsWith(text) ||
                w.Владелец.StartsWith(text));
        }

        public static void FileToArchive(int userId, int fileId, bool status)
        {
            var file = _db.Files.Where(w => w.id == fileId && w.user_id == userId).Single();

            file.archive = status;

            _db.SubmitChanges();

            if (status==true)
                UpdateStatusFile(userId, fileId, StatusFile.Draft);
        }

        public static void DeleteFile(int fileId)
        {
            var change = _db.FileChanges.Where(w => w.file_id == fileId);
            var file = _db.Files.Where(w => w.id == fileId).Single();
            var favorite = _db.FavoriteFiles.Where(w=>w.file_id==fileId);

            _db.FileChanges.DeleteAllOnSubmit(change);
            _db.FavoriteFiles.DeleteAllOnSubmit(favorite);
            _db.Files.DeleteOnSubmit(file);
            _db.SubmitChanges();
        }

        public static void UpdateFile(int userId, int fileId, string name, string comment)
        {
            var file = _db.Files.Where(f => f.id == fileId).Single();

            string changesComment = (file.comment != null && file.comment != comment && file.name != name) ? "Изменено имя и описание файла." : (file.name != name) ? "Изменено имя файла." : (file.comment != comment) ? "Изменено описание файла." : "";

            file.name = name;
            file.comment = comment;

            _db.SubmitChanges();

            if (changesComment != "")
                InsertFileChanges(userId, fileId, changesComment);
        }

        public static void InsertFile(string name, string extension, int size, Binary binary, bool reveiw, bool project, bool archive, int userId)
        {
            var newFile = new Files
            {
                name = name,
                extension = extension,
                size = size,
                binary = binary,
                review = reveiw,
                project = project,
                archive = archive,
                user_id = userId
            };

            _db.Files.InsertOnSubmit(newFile);
            _db.SubmitChanges();

            var file = _db.Files.Where(w => w.user_id == userId).ToList().Last();

            InsertFileChanges(userId, file.id, "Файл был загружен.");
        }

        public static IQueryable ChangesTable(int fileId)
        {
            return _db.FileChanges
                .Join(_db.Files, c => c.file_id, f => f.id, (c, f) => new { c, f })
                .Join(_db.Users, cc => cc.c.user_id, u => u.id, (cc, u) => new { cc, u })
                .OrderByDescending(o => o.cc.c.datetime_up)
                .Where(w => w.cc.f.id == fileId)
                .Select(s => new
                {
                    ЧтоИзменилось = s.cc.c.comment,
                    КтоИзменил = s.u.login,
                    ДатаИзменения = s.cc.c.datetime_up,
                });
        }

        public static IQueryable SearchChanges(int fileId, string text)
        {
            return _db.FileChanges
                .Join(_db.Files, c => c.file_id, f => f.id, (c, f) => new { c, f })
                .Join(_db.Users, cc => cc.c.user_id, u => u.id, (cc, u) => new { cc, u })
                .OrderByDescending(o => o.cc.c.datetime_up)
                .Where(w =>
                (w.cc.f.id == fileId) &&
                (w.u.login.StartsWith(text) ||
                w.cc.c.comment.StartsWith(text)))
                .Select(s => new
                {
                    ЧтоИзменилось = s.cc.c.comment,
                    КтоИзменил = s.u.login,
                    ДатаИзменения = s.cc.c.datetime_up,
                });
        }

        public static void InsertFileChanges(int userId, int fileId, string comment)
        {
            var newChange = new FileChanges
            {
                datetime_up = DateTime.Now,
                comment = comment,
                user_id = userId,
                file_id = fileId,
            };

            _db.FileChanges.InsertOnSubmit(newChange);
            _db.SubmitChanges();
        }

        public static bool CheckFavorite(int userId, int fileId)
        {
            return _db.FavoriteFiles.Where(w => w.file_id == fileId && w.user_id == userId).Any();
        }

        public static void InsertFavorite(int userId, int fileId)
        {
            var newFavorite = new FavoriteFiles
            {
                user_id = userId,
                file_id = fileId
            };

            _db.FavoriteFiles.InsertOnSubmit(newFavorite);
            _db.SubmitChanges();

            InsertFileChanges(userId, fileId, "Файл добавлен в избранное.");
        }

        public static void DeleteFavorite(int userId, int fileId)
        {
            var favorite = _db.FavoriteFiles.Where(w => w.file_id == fileId && w.user_id == userId).Single();

            _db.FavoriteFiles.DeleteOnSubmit(favorite);
            _db.SubmitChanges();

            InsertFileChanges(userId, fileId, "Файл убран из избранного.");
        }

        public static bool CheckFile(string name, string expansion, int userId)
        {
            return _db.Files.Where(w => w.user_id == userId && w.name == name && w.extension == expansion).Any();
        }

        public static void ReplaceFile(string name, string expansion, int size, Binary binary, int userId)
        {
            var file = _db.Files.Where(w => w.name == name && w.extension == expansion && w.user_id == userId).Single();

            file.binary = binary;
            file.size = size;
            file.project = false;
            file.review = false;

            _db.SubmitChanges();

            InsertFileChanges(userId, file.id, "Файл был обновлен.");
        }

        public static void UpdateStatusFile(int userId, int fileId, StatusFile statusFile)
        {
            var file = _db.Files.Where(w => w.id == fileId).Single();

            file.review = (statusFile == StatusFile.Review) ? true : false;
            file.project = (statusFile == StatusFile.Project) ? true : false;

            _db.SubmitChanges();

            InsertFileChanges(userId, file.id, (statusFile == StatusFile.Review) ? "Файл отправлен на проверку." : (statusFile == StatusFile.Project) ? "Изменен статус файла на \"Проект\"." : "Изменен статус файла на \"Черновик\".");
        }

        //============================================================================================================================

        public static void SPICALDELETE()
        {
            //_db.FileChanges.DeleteAllOnSubmit(_db.FileChanges);
            //_db.FavoriteFiles.DeleteAllOnSubmit(_db.FavoriteFiles);
            //_db.Files.DeleteAllOnSubmit(_db.Files);
            //_db.Users.DeleteAllOnSubmit(_db.Users);
            //_db.SubmitChanges();
        }
    }
}
