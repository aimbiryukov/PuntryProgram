using System;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Text.RegularExpressions;

namespace PuntryProgram.Classes
{
    public enum LevelAccessEnum
    {
        Admin,
        Editor,
        User
    }

    public enum StatusFileEnum
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
        public string statusFile;
        public bool archive;
        public int userId;

        public FileStruct(int fileId, string name, string comment, string extension, int size, Binary binary, DateTime dateTimeAT, DateTime dateTimeUP, string statusFile, bool archive, int userId)
        {
            this.fileId = fileId;
            this.name = name;
            this.comment = comment;
            this.extension = extension;
            this.size = size;
            this.binary = binary;
            this.dateTimeAT = dateTimeAT;
            this.dateTimeUP = dateTimeUP;
            this.statusFile = statusFile;
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
        public string levelName;

        public UserStruct(int userId, string name, string surname, string login, Guid password, DateTime dateTimeAT, Binary binary, string levelName)
        {
            this.userId = userId;
            this.name = name;
            this.surname = surname;
            this.login = login;
            this.password = password;
            this.dateTimeAT = dateTimeAT;
            this.binary = binary;
            this.levelName = levelName;
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
                .Join(_db.AccessLevels, u=>u.level_id,l=>l.id, (u,l)=> new { u,l })
                .OrderByDescending(o=>o.u.datetime_at)
                .Select(s => new
                {
                    s.u.id,
                    Логин = s.u.login,
                    Имя = s.u.name,
                    Фамилия = s.u.surname,
                    Статус = s.l.name,
                    s.u.datetime_at
                });
        }

        public static IQueryable SearchUsers(string text)
        {
            return _db.Users
                .Join(_db.AccessLevels, u => u.level_id, l => l.id, (u, l) => new { u, l })
                .OrderByDescending(o => o.u.datetime_at)
                .Select(s => new
                {
                    s.u.id,
                    Логин = s.u.login,
                    Имя = s.u.name,
                    Фамилия = s.u.surname,
                    Статус = s.l.name,
                    s.u.datetime_at
                })
                .Where(w =>
                w.Имя.StartsWith(text) ||
                w.Фамилия.StartsWith(text) ||
                w.Логин.StartsWith(text) ||
                w.Статус.StartsWith(text));
        }

        public static IQueryable AccessLevelsTable()
        {
            return _db.AccessLevels;
        }

        public static bool CheckRoot(int userId)
        {
            var file = _db.Users.Where(w => w.id == userId).Single();

            return file.root;
        }

        public static UserStruct GetUser(int userId)
        {
            return _db.Users
                .Join(_db.AccessLevels, u => u.level_id, l => l.id, (u, l) => new { u, l })
                .Where(w => w.u.id == userId)
                .Select(s => new UserStruct
                (
                    s.u.id,
                    s.u.name,
                    s.u.surname,
                    s.u.login,
                    (Guid)s.u.password,
                    (DateTime)s.u.datetime_at,
                    s.u.image,
                    s.l.name
                )).Single();
        }

        public static int GetUserId(string login)
        {
            return _db.Users.Where(w => w.login == login).Select(s => s.id).Single();
        }

        public static void InsertUser(string name, string surname, string login, Guid password, DateTime dateTimeAT, Binary binary, int levelId)
        {
            var newUser = new Users
            {
                name = name,
                surname = surname,
                login = login,
                password = password,
                datetime_at = dateTimeAT,
                image = binary,
                level_id = levelId
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

        public static void UpdateUser(int userId, string name, string surname, string login, Guid password, Binary binary, int levelId)
        {
            var user = _db.Users.Where(u => u.id == userId).Single();

            user.name = name;
            user.surname = surname;
            user.login = login;
            user.password = password;
            user.image = binary;
            user.level_id = levelId;

            _db.SubmitChanges();
        }

        public static IQueryable StatusFileTable()
        {
            return _db.StatusFile;
        }

        public static FileStruct GetFile(int fileId)
        {
            var dateTimeAT = (DateTime)_db.FileChanges.Where(w => w.file_id == fileId).ToList().First().datetime_up;
            var dateTimeUP = (DateTime)_db.FileChanges.Where(w => w.file_id == fileId).ToList().Last().datetime_up;

            return _db.FileChanges
                .Join(_db.Files, fc => fc.file_id, f => f.id, (fc, f) => new { fc, f })
                .Join(_db.StatusFile, ff => ff.f.status_file_id, sf => sf.id, (ff, sf) => new { ff, sf })
                .Where(w => w.ff.f.id == fileId)
                .Select(s => new FileStruct
                (
                    s.ff.f.id,
                    s.ff.f.name,
                    s.ff.f.comment,
                    s.ff.f.extension,
                    (int)s.ff.f.size,
                    s.ff.f.data,
                    dateTimeAT,
                    dateTimeUP,
                    s.sf.name,
                    s.ff.f.archive,
                    (int)s.ff.f.user_id
                )).First();
        }

        public static string CheckStatusFile(int fileId)
        {
            var file = _db.Files
                .Join(_db.StatusFile, f => f.status_file_id, sf => sf.id, (f, sf) => new { f, sf })
                .Where(w => w.f.id == fileId)
                .Single();

            return file.sf.name;
        }

        public static int CountFiles(int userId, StatusFileEnum statusFile)
        {
            var files = _db.Files
                .Join(_db.StatusFile, f => f.status_file_id, sf => sf.id, (f, sf) => new { f, sf })
                .Where(w => w.f.user_id == userId);

            return (statusFile == StatusFileEnum.My) ? files.Count() :
                (statusFile == StatusFileEnum.Project) ? files.Count(x => x.sf.name == "Проект") :
                files.Count(x => x.sf.name == "Черновик");
        }

        public static int SizeAllFiles(int userId)
        {
            var files = _db.Files.Where(w => w.user_id == userId);
            var result = 0;

            foreach (var file in files)
                result += (int)file.size;

            return result;
        }

        public static IQueryable FilesTable(int userId, StatusFileEnum statusFile)
        {
            return _db.Files
                .Join(_db.Users, f => f.user_id, u => u.id, (f, u) => new { f, u })
                .Join(_db.StatusFile, ff => ff.f.status_file_id, sf => sf.id, (ff, sf) => new { ff, sf })
                .Where(w =>
                (statusFile == StatusFileEnum.All) ? w.ff.f.archive == false :
                (statusFile == StatusFileEnum.Archive) ? w.ff.f.user_id == userId && w.ff.f.archive == true :
                (statusFile == StatusFileEnum.Project) ? w.ff.f.archive == false && w.sf.name == "Проект" :
                (statusFile == StatusFileEnum.Review) ? w.ff.f.archive == false && w.sf.name == "На проверке" :
                w.ff.f.user_id == userId && w.ff.f.archive == false)
                .Select(s => new
                {
                    s.ff.f.id,
                    Название = s.ff.f.name,
                    Расширение = s.ff.f.extension,
                    Описание = s.ff.f.comment,
                    Владелец = s.ff.u.login,
                    Статус = s.sf.name
                });
        }

        public static IQueryable FavoritesTable(int userId)
        {
            return _db.FavoriteFiles
                .Join(_db.Users, favorite1 => favorite1.user_id, u => u.id, (favorite1, u) => new { favorite1, u })
                .Join(_db.Files, favorite2 => favorite2.favorite1.file_id, f => f.id, (favorite2, f) => new { favorite2, f })
                .Join(_db.StatusFile, ff => ff.f.status_file_id, sf => sf.id, (ff, sf) => new { ff, sf })
                .Where(w => w.ff.favorite2.u.id == userId)
                .Select(s => new
                {
                    s.ff.f.id,
                    Название = s.ff.f.name,
                    Расширение = s.ff.f.extension,
                    Описание = s.ff.f.comment,
                    Владелец = s.ff.favorite2.u.login,
                    Статус = s.sf.name,
                });
        }

        public static IQueryable SearchFiles(int userId, string text, StatusFileEnum statusFile)
        {
            return _db.Files
                .Join(_db.Users, f => f.user_id, u => u.id, (f, u) => new { f, u })
                .Join(_db.StatusFile, ff => ff.f.status_file_id, sf => sf.id, (ff, sf) => new { ff, sf })
                .Where(w =>
                (statusFile == StatusFileEnum.All) ? w.ff.f.archive == false :
                (statusFile == StatusFileEnum.Archive) ? w.ff.f.user_id == userId && w.ff.f.archive == true :
                (statusFile == StatusFileEnum.Project) ? w.ff.f.archive == false && w.sf.name == "Проект" :
                (statusFile == StatusFileEnum.Review) ? w.ff.f.archive == false && w.sf.name == "На проверке" :
                w.ff.f.user_id == userId && w.ff.f.archive == false)
                .Select(s => new
                {
                    s.ff.f.id,
                    Название = s.ff.f.name,
                    Расширение = s.ff.f.extension,
                    Описание = s.ff.f.comment,
                    Владелец = s.ff.u.login,
                    Статус = s.sf.name
                })
                .Where(w =>
                w.Статус.StartsWith(text) ||
                w.Название.StartsWith(text) ||
                w.Описание.StartsWith(text) ||
                w.Владелец.StartsWith(text)||
                w.Расширение.StartsWith(text));
        }

        public static IQueryable SearchFavorites(int userId, string text)
        {
            return _db.FavoriteFiles
                .Join(_db.Users, favorite1 => favorite1.user_id, u => u.id, (favorite1, u) => new { favorite1, u })
                .Join(_db.Files, favorite2 => favorite2.favorite1.file_id, f => f.id, (favorite2, f) => new { favorite2, f })
                .Join(_db.StatusFile, ff => ff.f.status_file_id, sf => sf.id, (ff, sf) => new { ff, sf })
                .Where(w => w.ff.favorite2.u.id == userId)
                .Select(s => new
                {
                    s.ff.f.id,
                    Название = s.ff.f.name,
                    Расширение = s.ff.f.extension,
                    Описание = s.ff.f.comment,
                    Владелец = s.ff.favorite2.u.login,
                    Статус = s.sf.name
                })
                .Where(w =>
                w.Статус.StartsWith(text) ||
                w.Название.StartsWith(text) ||
                w.Описание.StartsWith(text) ||
                w.Владелец.StartsWith(text) ||
                w.Расширение.StartsWith(text));
        }

        public static void FileToArchive(int userId, int fileId, bool status)
        {
            var file = _db.Files.Where(w => w.id == fileId && w.user_id == userId).Single();

            file.archive = status;

            _db.SubmitChanges();

            if (status == true)
                UpdateStatusFile(userId, fileId, StatusFileEnum.Draft);
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
            var changesComment = (file.comment != null && file.comment != comment && file.name != name) ? "Изменено имя и описание файла." : (file.name != name) ? "Изменено имя файла." : (file.comment != comment) ? "Изменено описание файла." : "";

            file.name = name;
            file.comment = comment;

            _db.SubmitChanges();

            if (changesComment != "")
                InsertFileChanges(userId, fileId, changesComment);
        }

        public static void InsertFile(string name, string extension, int size, Binary binary, bool archive, int userId)
        {
            var status = _db.StatusFile.Where(w => w.name == "Черновик").Single();

            var newFile = new Files
            {
                name = name,
                extension = extension,
                size = size,
                data = binary,
                archive = archive,
                user_id = userId,
                status_file_id = status.id
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
                    s.cc.c.comment,
                    s.u.login,
                    s.cc.c.datetime_up,
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
                    s.cc.c.comment,
                    s.u.login,
                    s.cc.c.datetime_up,
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
            var status = _db.StatusFile.Where(w => w.name == "Черновик").Single();

            file.data = binary;
            file.size = size;
            file.status_file_id = status.id;

            _db.SubmitChanges();

            InsertFileChanges(userId, file.id, "Файл был обновлен.");
        }

        public static void UpdateStatusFile(int userId, int fileId, StatusFileEnum statusFile)
        {
            var status = _db.StatusFile
                .Where(w =>
                (statusFile == StatusFileEnum.Project) ? w.name == "Проект" :
                (statusFile == StatusFileEnum.Review) ? w.name == "На проверке" : w.name == "Черновик"
                ).Single();

            var file = _db.Files
                .Join(_db.StatusFile, f => f.status_file_id, sf => sf.id, (f, sf) => new { f, sf })
                .Where(w => w.f.id == fileId).Single();

            file.f.status_file_id = status.id;

            _db.SubmitChanges();

            InsertFileChanges(userId, file.f.id, (statusFile == StatusFileEnum.Review) ? "Файл отправлен на проверку." : (statusFile == StatusFileEnum.Project) ? "Изменен статус файла на \"Проект\"." : "Изменен статус файла на \"Черновик\".");
        }
    }
}
