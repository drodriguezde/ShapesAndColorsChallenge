using ShapesAndColorsChallenge.Class;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.DataBase.Types;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ShapesAndColorsChallenge.DataBase.Controllers
{
    internal static class ControllerRanking
    {
        #region CONST

        static readonly List<Tuple<string, string>> PLAYERS = new()
        {
            new Tuple<string, string>(Const.PLAYER_NAME, "en"),
            new Tuple<string, string>("Ahmad Zahir", "af"),
            new Tuple<string, string>("Sima Samar", "af"),
            new Tuple<string, string>("Boualem Belhadj", "ag"),
            new Tuple<string, string>("Leila Bekhti", "ag"),
            new Tuple<string, string>("Arben Xhaka", "al"),
            new Tuple<string, string>("Albana Molla", "al"),
            new Tuple<string, string>("Aram Baghdasaryan", "am"),
            new Tuple<string, string>("Lusine Karapetyan", "am"),
            new Tuple<string, string>("Marc Font", "an"),
            new Tuple<string, string>("Laia Comas", "an"),
            new Tuple<string, string>("Simão Gomes", "ao"),
            new Tuple<string, string>("Isabel Pereira", "ao"),
            new Tuple<string, string>("Mateo Fernández", "ar"),
            new Tuple<string, string>("Valentina González", "ar"),
            new Tuple<string, string>("Liam Nguyen", "as"),
            new Tuple<string, string>("Emily Patel", "as"),
            new Tuple<string, string>("David Huber", "au"),
            new Tuple<string, string>("Sarah Gruber", "au"),
            new Tuple<string, string>("Ahmed Al Khalifa", "ba"),
            new Tuple<string, string>("Mariam Abdulrahman", "ba"),
            new Tuple<string, string>("Ryan Clarke", "bb"),
            new Tuple<string, string>("Olivia Maynard", "bb"),
            new Tuple<string, string>("Thato Banda", "bc"),
            new Tuple<string, string>("Kagiso Nkwe", "bc"),
            new Tuple<string, string>("Joris Van Dijck", "be"),
            new Tuple<string, string>("Annelies Verstraete", "be"),
            new Tuple<string, string>("Mateo Mamani", "bl"),
            new Tuple<string, string>("Carla Flores Mendoza", "bl"),
            new Tuple<string, string>("Tunde Adeoti", "bn"),
            new Tuple<string, string>("Nadège Agbodjan", "bn"),
            new Tuple<string, string>("Ivan Petrovich", "bo"),
            new Tuple<string, string>("Alina Kovalchuk", "bo"),
            new Tuple<string, string>("Thiago Oliveira", "br"),
            new Tuple<string, string>("Camila Souza Freitas", "br"),
            new Tuple<string, string>("Tshering Dorji", "bt"),
            new Tuple<string, string>("Tandin Zam", "bt"),
            new Tuple<string, string>("Georgi Petrov", "bu"),
            new Tuple<string, string>("Ana Petrova", "bu"),
            new Tuple<string, string>("Jean Nduwayo", "by"),
            new Tuple<string, string>("Emmanuella Nkurunziza", "by"),
            new Tuple<string, string>("William Jones", "ca"),
            new Tuple<string, string>("Emily Davis", "ca"),
            new Tuple<string, string>("Ahmadou Mahamat", "cd"),
            new Tuple<string, string>("Hadja Souleymane", "cd"),
            new Tuple<string, string>("Joseph Nkosi", "cg"),
            new Tuple<string, string>("Marie-Claire Mwanza", "cg"),
            new Tuple<string, string>("Diego Fernández", "ci"),
            new Tuple<string, string>("Carolina Rodríguez", "ci"),
            new Tuple<string, string>("Jean-Paul Nkembe", "cm"),
            new Tuple<string, string>("Marie-Claire Tchoumi", "cm"),
            new Tuple<string, string>("Zhang Wei", "zh"),
            new Tuple<string, string>("Wang Jing", "zh"),
            new Tuple<string, string>("Carlos Muñoz", "co"),
            new Tuple<string, string>("Ana Rodríguez", "co"),
            new Tuple<string, string>("Carlos Solano", "css"),
            new Tuple<string, string>("María Torres", "css"),
            new Tuple<string, string>("David Mbai-Bataka", "ct"),
            new Tuple<string, string>("Pauline Yakoma", "ct"),
            new Tuple<string, string>("José Martí", "cu"),
            new Tuple<string, string>("Celia Sánchez", "cu"),
            new Tuple<string, string>("Adilson Lopes", "cv"),
            new Tuple<string, string>("Catarina Tavares", "cv"),
            new Tuple<string, string>("Andreas Nicolaou", "cy"),
            new Tuple<string, string>("Eleni Papadopoulos", "cy"),
            new Tuple<string, string>("Jan Novak", "cs"),
            new Tuple<string, string>("Jana Kralova", "cs"),
            new Tuple<string, string>("Lukas Bauer", "de"),
            new Tuple<string, string>("Emma Schulz", "de"),
            new Tuple<string, string>("Ahmed Ali", "dj"),
            new Tuple<string, string>("Aisha Abdi", "dj"),
            new Tuple<string, string>("Lars Hansen", "da"),
            new Tuple<string, string>("Maria Andersen", "da"),
            new Tuple<string, string>("Carlos Ramírez", "dr"),
            new Tuple<string, string>("Julia García", "dr"),
            new Tuple<string, string>("Francisco González", "ec"),
            new Tuple<string, string>("María Pérez", "ec"),
            new Tuple<string, string>("Ahmed Hassan", "eg"),
            new Tuple<string, string>("Nour Abdel Aziz", "eg"),
            new Tuple<string, string>("Liam O'Brien", "ei"),
            new Tuple<string, string>("Aoife Kelly", "ei"),
            new Tuple<string, string>("Basilio Osa", "ek"),
            new Tuple<string, string>("Martina Nchama", "ek"),
            new Tuple<string, string>("Kristjan Kallas", "enn"),
            new Tuple<string, string>("Maria-Liisa Kubo", "enn"),
            new Tuple<string, string>("Alejandro Fernández", "es"),
            new Tuple<string, string>("Ana López", "es"),
            new Tuple<string, string>("Abdiwahid Ali", "et"),
            new Tuple<string, string>("Tsehay Moges", "et"),
            new Tuple<string, string>("Aleksi Järvinen", "fi"),
            new Tuple<string, string>("Emma Hakala", "fi"),
            new Tuple<string, string>("Antoine Dubois", "fr"),
            new Tuple<string, string>("Sophie Martin", "fr"),
            new Tuple<string, string>("Pierre Mabika", "ga"),
            new Tuple<string, string>("Solange Nkoghe", "ga"),
            new Tuple<string, string>("William Johnson", "en"),
            new Tuple<string, string>("Charlotte Thompson", "en"),
            new Tuple<string, string>("David Kipiani", "gg"),
            new Tuple<string, string>("Tamar Mchedlidze", "gg"),
            new Tuple<string, string>("Kwame Mensah", "gh"),
            new Tuple<string, string>("Abena Yeboah", "gh"),
            new Tuple<string, string>("Lamin Jallow", "gm"),
            new Tuple<string, string>("Isatou Jagne", "gm"),
            new Tuple<string, string>("Andreas Papandreou", "gr"),
            new Tuple<string, string>("Sofia Antoniou", "gr"),
            new Tuple<string, string>("Carlos García", "gt"),
            new Tuple<string, string>("María Rodríguez", "gt"),
            new Tuple<string, string>("Mamadou Diallo", "gv"),
            new Tuple<string, string>("Fanta Camara", "gv"),
            new Tuple<string, string>("Rajiv Persaud", "gy"),
            new Tuple<string, string>("Kamla Singh", "gy"),
            new Tuple<string, string>("Jean-Louis Charles", "ha"),
            new Tuple<string, string>("Fabienne Auguste", "ha"),
            new Tuple<string, string>("Carlos Fuentes", "ho"),
            new Tuple<string, string>("Carmen Flores", "ho"),
            new Tuple<string, string>("Ivan Kovacic", "hr"),
            new Tuple<string, string>("Ivana Markovic", "hr"),
            new Tuple<string, string>("Gábor Nagy", "hu"),
            new Tuple<string, string>("Viktória Szabó", "hu"),
            new Tuple<string, string>("Gudmundur Jonsson", "ic"),
            new Tuple<string, string>("Kristin Jonsdottir", "ic"),
            new Tuple<string, string>("Irfan Rachman", "id"),
            new Tuple<string, string>("Rina Amelia sari", "id"),
            new Tuple<string, string>("Aryan Kapoor", "in"),
            new Tuple<string, string>("Aanya Gupta", "in"),
            new Tuple<string, string>("Ali Najafi", "ir"),
            new Tuple<string, string>("Fatemeh Jafari", "ir"),
            new Tuple<string, string>("Adam Cohen", "is"),
            new Tuple<string, string>("Noa Levy", "is"),
            new Tuple<string, string>("Marco Rossi", "it"),
            new Tuple<string, string>("Martina Conti", "it"),
            new Tuple<string, string>("Kouassi Konan", "iv"),
            new Tuple<string, string>("Aïssata Traoré", "iv"),
            new Tuple<string, string>("Ahmed Abbas", "iz"),
            new Tuple<string, string>("Fatima Kareem", "iz"),
            new Tuple<string, string>("Winston Clarke", "jm"),
            new Tuple<string, string>("Kimberly Williams", "jm"),
            new Tuple<string, string>("Ahmad Khalid", "jo"),
            new Tuple<string, string>("Rania Abu-Abdullah", "jo"),
            new Tuple<string, string>("Yuto Nakamura", "ja"),
            new Tuple<string, string>("Aya Nakamura", "ja"),
            new Tuple<string, string>("David Mwangi", "ke"),
            new Tuple<string, string>("Joyce Akinyi", "ke"),
            new Tuple<string, string>("Emilbek Turgunov", "kg"),
            new Tuple<string, string>("Gulzada Ismailova", "kg"),
            new Tuple<string, string>("Minho Lee", "kn"),
            new Tuple<string, string>("Soo-yeon Choi", "kn"),
            new Tuple<string, string>("Kim Min-seok", "ko"),
            new Tuple<string, string>("Choi Soo-yeon", "ko"),
            new Tuple<string, string>("Ahmed Al-Abdullah", "ku"),
            new Tuple<string, string>("Maryam Al-Faris", "ku"),
            new Tuple<string, string>("Daulet Akhmetov", "kz"),
            new Tuple<string, string>("Saltanat Abisheva", "kz"),
            new Tuple<string, string>("Ali Najjar", "le"),
            new Tuple<string, string>("Farah Harb", "le"),
            new Tuple<string, string>("Janis Liepins", "lg"),
            new Tuple<string, string>("Ieva Kalnina", "lg"),
            new Tuple<string, string>("Jonas Kavaliauskas", "lh"),
            new Tuple<string, string>("Laura Mikalauskaite", "lh"),
            new Tuple<string, string>("William Johnson", "li"),
            new Tuple<string, string>("Mary Davis", "li"),
            new Tuple<string, string>("Tomáš Novák", "lo"),
            new Tuple<string, string>("Zuzana Vargaová", "lo"),
            new Tuple<string, string>("Lukas Meier", "ls"),
            new Tuple<string, string>("Nina Schmid", "ls"),
            new Tuple<string, string>("Nthabeleng Thaele", "lt"),
            new Tuple<string, string>("Mamosa Makoro", "lt"),
            new Tuple<string, string>("Hugo Muller", "lu"),
            new Tuple<string, string>("Sophie Dubois", "lu"),
            new Tuple<string, string>("Mohamed Khalifa", "ly"),
            new Tuple<string, string>("Aisha Abdullah", "ly"),
            new Tuple<string, string>("Andry Rakotondravola", "ma"),
            new Tuple<string, string>("Sariaka Rakotondrazaka", "ma"),
            new Tuple<string, string>("Nicolae Ceban", "md"),
            new Tuple<string, string>("Ana Munteanu", "md"),
            new Tuple<string, string>("Chikondi Banda", "mi"),
            new Tuple<string, string>("Mwai Matemba", "mi"),
            new Tuple<string, string>("Nikola Vukčević", "mj"),
            new Tuple<string, string>("Ana Đurišić", "mj"),
            new Tuple<string, string>("Filip Georgievski", "mk"),
            new Tuple<string, string>("Elena Stojanova", "mk"),
            new Tuple<string, string>("Amadou Keita", "ml"),
            new Tuple<string, string>("Mariam Diallo", "ml"),
            new Tuple<string, string>("Louis Blanchard", "mn"),
            new Tuple<string, string>("Camille Roussel", "mn"),
            new Tuple<string, string>("Youssef El Mekki", "mo"),
            new Tuple<string, string>("Leila Amrani", "mo"),
            new Tuple<string, string>("Ravi Naidu", "mp"),
            new Tuple<string, string>("Alisha Gunesh", "mp"),
            new Tuple<string, string>("Mohamed Cheikh", "mr"),
            new Tuple<string, string>("Aminetou Kane", "mr"),
            new Tuple<string, string>("Marco Vella", "mt"),
            new Tuple<string, string>("Francesca Schembri", "mt"),
            new Tuple<string, string>("Mohamed Abdullah", "mv"),
            new Tuple<string, string>("Fathimath Ibrahim", "mv"),
            new Tuple<string, string>("Juan García", "mx"),
            new Tuple<string, string>("Sofia Ramírez", "mx"),
            new Tuple<string, string>("Aarav Patel", "ng"),
            new Tuple<string, string>("Aashi Kapoor", "ng"),
            new Tuple<string, string>("Chuka Eze", "ni"),
            new Tuple<string, string>("Amina Mohammed", "ni"),
            new Tuple<string, string>("Daan de Jong", "nl"),
            new Tuple<string, string>("Femke de Vries", "nl"),
            new Tuple<string, string>("Erik Andersen", "no"),
            new Tuple<string, string>("Sofie Larsen", "no"),
            new Tuple<string, string>("Ranomi Wisseh", "ns"),
            new Tuple<string, string>("Cedric Telting", "ns"),
            new Tuple<string, string>("Carlos Gutiérrez", "nu"),
            new Tuple<string, string>("Gabriela Pérez", "nu"),
            new Tuple<string, string>("Liam Campbell", "nz"),
            new Tuple<string, string>("Isla Martin", "nz"),
            new Tuple<string, string>("Omar Hassan al-Bashir", "od"),
            new Tuple<string, string>("Aisha Musa el-Said", "od"),
            new Tuple<string, string>("Diego González", "pa"),
            new Tuple<string, string>("María Gómez", "pa"),
            new Tuple<string, string>("Alejandro Torres", "pe"),
            new Tuple<string, string>("Valeria Chavez", "pe"),
            new Tuple<string, string>("Muhammad Ali", "pk"),
            new Tuple<string, string>("Ayesha Malik", "pk"),
            new Tuple<string, string>("Adam Kowalski", "pl"),
            new Tuple<string, string>("Anna Szymańska", "pl"),
            new Tuple<string, string>("Juan García", "pm"),
            new Tuple<string, string>("María Rodríguez", "pm"),
            new Tuple<string, string>("Kofi Boas", "pp"),
            new Tuple<string, string>("Bani Tau", "pp"),
            new Tuple<string, string>("Mohammad Abu-Ali", "ps"),
            new Tuple<string, string>("Fatima Hamdan", "ps"),
            new Tuple<string, string>("Pedro Silva", "pt"),
            new Tuple<string, string>("Sofia Santos", "pt"),
            new Tuple<string, string>("Abubacar Baldé", "pu"),
            new Tuple<string, string>("Fátima Djau", "pu"),
            new Tuple<string, string>("Khalid al-Mohannadi", "qa"),
            new Tuple<string, string>("Mariam al-Kuwari", "qa"),
            new Tuple<string, string>("Nikola Petrovic", "ri"),
            new Tuple<string, string>("Jovana Stojanovic", "ri"),
            new Tuple<string, string>("Andrei Popescu", "ro"),
            new Tuple<string, string>("Elena Stanescu", "ro"),
            new Tuple<string, string>("Miguel Santos", "rp"),
            new Tuple<string, string>("Ana Garcia", "rp"),
            new Tuple<string, string>("Aleksandr Ivanov", "ru"),
            new Tuple<string, string>("Anastasia Ivanova", "ru"),
            new Tuple<string, string>("ean Baptiste Niyonzima", "rw"),
            new Tuple<string, string>("Marie Claire Uwamahoro", "rw"),
            new Tuple<string, string>("Omar Al-Malik", "sa"),
            new Tuple<string, string>("Aisha Al-Saud", "sa"),
            new Tuple<string, string>("Erik Andersson", "sv"),
            new Tuple<string, string>("Sofia Johansson", "sv"),
            new Tuple<string, string>("Thabo Mphahlele", "sf"),
            new Tuple<string, string>("Lerato van der Merwe", "sf"),
            new Tuple<string, string>("Moussa Diop", "sg"),
            new Tuple<string, string>("Aminata Fall", "sg"),
            new Tuple<string, string>("Marko Novak", "si"),
            new Tuple<string, string>("Petra Kovačič", "si"),
            new Tuple<string, string>("Mohamed Kamara", "sl"),
            new Tuple<string, string>("Aminata Bangura", "sl"),
            new Tuple<string, string>("Marco Rossi", "sm"),
            new Tuple<string, string>("Sofia Ricci", "sm"),
            new Tuple<string, string>("Ethan Lim", "sn"),
            new Tuple<string, string>("Isabel Lee", "sn"),
            new Tuple<string, string>("Mohamed Ali", "so"),
            new Tuple<string, string>("Aisha Hassan", "so"),
            new Tuple<string, string>("Michael Thomas", "st"),
            new Tuple<string, string>("Gabriella Jules-Josep", "st"),
            new Tuple<string, string>("Mohamed Ahmed", "su"),
            new Tuple<string, string>("Salma Elhassan", "su"),
            new Tuple<string, string>("Carlos Mejía", "svv"),
            new Tuple<string, string>("Marta Hernández", "svv"),
            new Tuple<string, string>("Omar Al-Said", "sy"),
            new Tuple<string, string>("Noura Aboud", "sy"),
            new Tuple<string, string>("Lukas Müller", "sz"),
            new Tuple<string, string>("Sophia Schmid", "sz"),
            new Tuple<string, string>("Adrian Williams", "td"),
            new Tuple<string, string>("Keisha Alexander", "td"),
            new Tuple<string, string>("Sombat Chaiyong", "th"),
            new Tuple<string, string>("Juthamas Siriwan", "th"),
            new Tuple<string, string>("Jamshed Safarov", "ti"),
            new Tuple<string, string>("Aziza Davronova", "ti"),
            new Tuple<string, string>("Sione Finau", "tn"),
            new Tuple<string, string>("Mele Folau", "tn"),
            new Tuple<string, string>("Koffi Adanlete", "to"),
            new Tuple<string, string>("Yaovi Akoussah-Zanou", "to"),
            new Tuple<string, string>("Emre Kaya", "tr"),
            new Tuple<string, string>("Selin Yılmaz", "tr"),
            new Tuple<string, string>("Ahmed Haddad", "ts"),
            new Tuple<string, string>("Amina Boubaker", "ts"),
            new Tuple<string, string>("Amanmuhammet Atayev", "tx"),
            new Tuple<string, string>("Guljemal Yagshimuradova", "tx"),
            new Tuple<string, string>("Juma Hassan", "tz"),
            new Tuple<string, string>("Fatuma Mwamba", "tz"),
            new Tuple<string, string>("Charles Mukasa", "ug"),
            new Tuple<string, string>("Grace Akello", "ug"),
            new Tuple<string, string>("Andriy Hryvko", "up"),
            new Tuple<string, string>("Oksana Baiul", "up"),
            new Tuple<string, string>("William Johnson", "us"),
            new Tuple<string, string>("Samantha Smith", "us"),
            new Tuple<string, string>("Issa Zongo", "uv"),
            new Tuple<string, string>("Mariam Sankara", "uv"),
            new Tuple<string, string>("Santiago Rodríguez", "uy"),
            new Tuple<string, string>("Valentina González", "uy"),
            new Tuple<string, string>("Abdullojon Tuychiev", "uz"),
            new Tuple<string, string>("Nigora Omonova", "uz"),
            new Tuple<string, string>("Carlos Pérez", "ve"),
            new Tuple<string, string>("Isabel Rodríguez", "ve"),
            new Tuple<string, string>("Nguyen Van", "vm"),
            new Tuple<string, string>("Le Thi", "vm"),
            new Tuple<string, string>("Johannes van Wyk", "wa"),
            new Tuple<string, string>("Martha Nakale", "wa"),
            new Tuple<string, string>("Tama Fa'asoa", "ws"),
            new Tuple<string, string>("Moana Leota", "ws"),
            new Tuple<string, string>("Sibusiso Mhlanga", "wz"),
            new Tuple<string, string>("Zinhle Mkhonta", "wz"),
            new Tuple<string, string>("Abdul Rahman Al-Samawi", "ym"),
            new Tuple<string, string>("Fatima Saleh Al-Qadhi", "ym"),
            new Tuple<string, string>("David Mbewe", "za"),
            new Tuple<string, string>("Chileshe Chanda", "za"),
            new Tuple<string, string>("Tendai Dube", "zi"),
            new Tuple<string, string>("Chipo Ndlovu", "zi")
        };

        #endregion

        #region METHODS

        /// <summary>
        /// Genera la tabla de Ranking en caso de no existir.
        /// </summary>
        internal static void Deploy()
        {
            //DataBaseManager.Connection.DropTable<Player>();
            //DataBaseManager.Connection.DropTable<Ranking>();
            DataBaseManager.Connection.CreateTable<Player>();
            DataBaseManager.Connection.CreateTable<Ranking>();

            if (Any())
                return;

            DataBaseManager.Connection.BeginTransaction();/*Usamos una transacción para insertar todos los registros de golpe en bulk*/

            for (int i = 0; i < PLAYERS.Count; i++)
            {
                Player player = new()
                {
                    PlayerID = i + 1,
                    Name = PLAYERS[i].Item1,
                    Country = PLAYERS[i].Item2,
                    IsPlayer = PLAYERS[i].Item1 == Const.PLAYER_NAME
                };

                DataBaseManager.Connection.Insert(player);

                foreach (GameMode gameMode in System.Enum.GetValues(typeof(GameMode)))
                    if (gameMode != GameMode.None)
                    {
                        Ranking ranking = new()
                        {
                            PlayerID = player.PlayerID,
                            GameMode = gameMode,
                            Win = 0,
                            Lose = 0
                        };
                        DataBaseManager.Connection.Insert(ranking);
                    }
            }

            DataBaseManager.Connection.Commit();
        }

        /// <summary>
        /// Obtiene todos los ranking.
        /// </summary>
        /// <returns></returns>
        static List<Ranking> Get()
        {
            return DataBaseManager.Connection.Table<Ranking>().ToList();
        }

        /// <summary>
        /// Obtiene un listado con todos los ranking filtrados por modo de juego.
        /// </summary>
        /// <returns></returns>
        static List<Ranking> Get(GameMode gameMode)
        {
            return DataBaseManager.Connection.Table<Ranking>().Where(t => t.GameMode == gameMode).ToList();
        }

        internal static bool Any()
        {
            return DataBaseManager.Connection.Table<Ranking>().Any();
        }

        internal static void Reset()
        {
            List<Ranking> rankings = Get();

            foreach (Ranking ranking in rankings)
            {
                ranking.Win = 0;
                ranking.Lose = 0;
            }

            DataBaseManager.Connection.UpdateAll(rankings, true);
        }

        /// <summary>
        /// Obtiene todos los jugadores del ranking de un modo de juego ordenados por su posición.
        /// </summary>
        /// <param name="gameMode"></param>
        /// <returns></returns>
        internal static List<RankingByGameMode> GetWithPlayers(GameMode gameMode)
        {
            return (from player in ControllerPlayer.Get()
                    join ranking in Get(gameMode) on player.PlayerID equals ranking.PlayerID
                    orderby ranking.Win * 3 - ranking.Lose * 2 descending/*El primero el que tiene mayor puntuación*/
                    select new RankingByGameMode
                    {
                        Name = player.Name,
                        Country = player.Country,
                        IsPlayer = player.IsPlayer,
                        Position = ranking.Win * 3 - ranking.Lose * 2,
                        Win = ranking.Win
                    }).ToList();
        }

        /// <summary>
        /// Obtiene los ranking en los diferentes modos de juego del jugador.
        /// </summary>
        /// <returns></returns>
        internal static List<RankingByGameMode> GetUserRanking()
        {
            return (from player in ControllerPlayer.Get()
                    join ranking in Get() on player.PlayerID equals ranking.PlayerID
                    orderby ranking.Win * 3 - ranking.Lose * 2 descending/*El primero el que tiene mayor puntuación*/
                    where player.IsPlayer
                    select new RankingByGameMode
                    {
                        Name = player.Name,
                        Country = player.Country,
                        Position = ranking.Win * 3 - ranking.Lose * 2,
                        Win = ranking.Win
                    }).ToList();
        }

        #endregion
    }
}
