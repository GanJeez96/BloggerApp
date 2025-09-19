CREATE TABLE Posts(
                      id bigint NOT NULL AUTO_INCREMENT PRIMARY KEY,
                      authorId bigint NOT NULL,
                      title varchar(100),
                      description varchar(500),
                      content text,

                      FOREIGN KEY (authorId) REFERENCES Authors(id)
);