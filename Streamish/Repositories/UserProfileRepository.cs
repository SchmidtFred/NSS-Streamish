using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Streamish.Models;
using Streamish.Utils;

namespace Streamish.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT Id, Name, Email, ImageUrl, DateCreated
                              FROM UserProfile";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var profiles = new List<UserProfile>();
                        while (reader.Read())
                        {
                            profiles.Add(new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                                DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                            });
                        }

                        return profiles;
                    }
                }
            }
        }

        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT Id, Name, Email, ImageUrl, DateCreated
                              FROM UserProfile
                             WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        UserProfile profile = null;
                        if (reader.Read())
                        {
                            profile = new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                                DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                            };
                        }

                        return profile;
                    }
                }
            }
        }

        public UserProfile GetByIdWithVideos(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT up.Id AS UserId, up.Name AS UserName, up.Email AS UserEmail, up.ImageUrl AS UserImageUrl, up.DateCreated AS UserDateCreated,
   
                                   v.Id AS VideoId, v.Title, v.Description, v.Url, v.DateCreated AS VideoDateCreated,

                                   c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId,

                                   cup.Name AS CommentUserName, cup.Email AS CommentUserEmail, cup.ImageUrl AS CommentUserImageUrl, cup.DateCreated AS CommentUserDateCreated
                             FROM UserProfile up
                                  LEFT JOIN Video v ON v.UserProfileId = up.Id
                                  LEFT JOIN Comment c ON c.VideoId = v.Id
                                  LEFT JOIN UserProfile cup ON c.UserProfileId = cup.Id
                            WHERE up.Id = @id
                         ORDER BY v.DateCreated";

                    DbUtils.AddParameter(cmd, "@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        UserProfile profile = null;
                        while (reader.Read())
                        {
                            //create profile if not created already
                            if (profile == null)
                            {
                                profile = new UserProfile()
                                {
                                    Id = DbUtils.GetInt(reader, "UserId"),
                                    Name = DbUtils.GetString(reader, "UserName"),
                                    Email = DbUtils.GetString(reader, "UserEmail"),
                                    ImageUrl = DbUtils.GetString(reader, "UserImageUrl"),
                                    DateCreated = DbUtils.GetDateTime(reader, "UserDateCreated"),
                                    Videos = new List<Video>(),
                                };
                            }

                            //grab id of this sql row's video
                            var videoId = DbUtils.GetInt(reader, "Videoid");

                            //store it if it exists
                            var existingVideo = profile.Videos.FirstOrDefault(v => v.Id == videoId);
                            //make a video object if it doesn't
                            if (existingVideo == null)
                            {
                                existingVideo = new Video()
                                {
                                    Id = videoId,
                                    Title = DbUtils.GetString(reader, "Title"),
                                    Description = DbUtils.GetString(reader, "Description"),
                                    DateCreated = DbUtils.GetDateTime(reader, "VideoDateCreated"),
                                    Url = DbUtils.GetString(reader, "Url"),
                                    UserProfileId = DbUtils.GetInt(reader, "UserId"),
                                    Comments = new List<Comment>(),
                                };

                                //add this new video to the list in our profile
                                profile.Videos.Add(existingVideo);
                            }

                            //find if there are any comments on this row, if so add it to the video object
                            if (DbUtils.IsNotDbNull(reader, "CommentId"))
                            {
                                existingVideo.Comments.Add(new Comment()
                                {
                                    Id = DbUtils.GetInt(reader, "CommentId"),
                                    Message = DbUtils.GetString(reader, "Message"),
                                    VideoId = videoId,
                                    UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId"),
                                    UserProfile = new UserProfile()
                                    {
                                        Id = DbUtils.GetInt(reader, "CommentUserProfileId"),
                                        Name = DbUtils.GetString(reader, "CommentUserName"),
                                        Email = DbUtils.GetString(reader, "CommentUserEmail"),
                                        ImageUrl = DbUtils.GetString(reader, "CommentUserImageUrl"),
                                        DateCreated = DbUtils.GetDateTime(reader, "CommentUserDateCreated")
                                    }
                                });
                            }
                        }

                        //now done return
                        return profile;
                    }
                }
            }
        }

        public void Add(UserProfile profile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            INSERT INTO UserProfile (Name, Email, ImageUrl, DateCreated)
                            OUTPUT INSERTED.ID
                            VALUES (@Name, @Email, @ImageUrl, @DateCreated)";

                    DbUtils.AddParameter(cmd, "@Name", profile.Name);
                    DbUtils.AddParameter(cmd, "@Email", profile.Email);
                    DbUtils.AddParameter(cmd, "@ImageUrl", profile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@DateCreated", profile.DateCreated);

                    profile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(UserProfile profile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE UserProfile
                               SET Name = @name,
                                   Email = @email,
                                   ImageUrl = @imageUrl,
                                   DateCreated = @dateCreated
                             WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@name", profile.Name);
                    DbUtils.AddParameter(cmd, "@email", profile.Email);
                    DbUtils.AddParameter(cmd, "@imageUrl", profile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@dateCreated", profile.DateCreated);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM UserProfile WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
