create database GameLive;

use GameLive
CREATE TABLE SAVE_GAMES(
  ID_GAME   INT 	 IDENTITY(1,1),
  DATE		DATETIME    NOT NULL,   
  COMMENT	VARCHAR(50)      NOT NULL
  PRIMARY KEY (ID_GAME)
)

CREATE TABLE MAP_GAMES(
  ID_GAME   INT			NOT NULL,
  X			TINYINT		NOT NULL,
  Y			TINYINT		NOT NULL,
  ALIVE		BIT			NOT NULL
)


--STATUS
CREATE TABLE LOG_GAMES(
  ID_GAME   INT 	 IDENTITY(1,1),
  DATE		DATETIME    NOT NULL,   
  COMMENT	VARCHAR(50)      NOT NULL
  PRIMARY KEY (ID_GAME)
)

CREATE TABLE LOG_MAP_GAMES(
  ID_GAME   INT			NOT NULL,
  X			TINYINT		NOT NULL,
  Y			TINYINT		NOT NULL,
  ALIVE		BIT			NOT NULL
)

