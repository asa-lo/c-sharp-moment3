// Åsa Lodesjö - Momment 3
// .NET CORE 3.0

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace guestbook
{
        public class guestBook
    {
        private List<Post> posts = new List<Post>();

        string path = "guestbook.data";
        public guestBook(){ 
            if(File.Exists(@path)){
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(@path, FileMode.Open, FileAccess.Read); //Spara data binärt

                while(stream.Position<stream.Length){   
                    Post postNew = (Post)formatter.Deserialize(stream); 
                    posts.Add(postNew);
                }
                stream.Close();
            }
        }
        public Post addPost(Post post){ // Metod för att lägga till nytt inlägg
            posts.Add(post);
            serializePosts();         
            return post;
        }
            
        public int delPost(int index){ //Metod för att radera inlägg baserat på index input
            posts.RemoveAt(index);
            serializePosts();
            return index;
        }

        public List<Post> getPosts(){ // Metod för att returnera alla inlägg i filen guestbook.data
            return posts;
        }

        private void serializePosts() //metod för att serializera input in till filen
        {
            IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(@path, FileMode.Create, FileAccess.Write);
                foreach(Post obj in posts){
                    formatter.Serialize(stream, obj);
                }
            stream.Close();
        }
    }
    [Serializable]
    public class Post //Class med fields
    {
        private string Name; 
        private string Comment;       
        public string name
        {
            set {this.Name = value;}
            get {return this.Name;}
        }
        public string comment
        {
            set {this.Comment = value;}
            get {return this.Comment;}
        }
    }
    class Program
    {
        static void Main(string[] args) //Huvudprogram
        {
 
            guestBook newApp = new guestBook(); // skapa ny instans i gästboken.
            int i=0;

            while(true){
                Console.Clear();Console.CursorVisible = false;
                Console.WriteLine("MY GUESTBOOK\n\n");

                Console.WriteLine("1. Lägg till ett inlägg");
                Console.WriteLine("2. Ta bort ett inlägg\n");
                Console.WriteLine("X. Avsluta\n");

                i=0;
                foreach(Post post in newApp.getPosts()){
                    Console.WriteLine("[" + i++ + "] " + post.name + " - " + post.comment); //Loopa igenom alla inlägg i listan posts
                }

                string inp = Console.ReadLine().ToLower();
                switch (inp) {
                    case "1":
                        Console.CursorVisible = true; 
                        Console.Write("Ange ditt namn: ");
                        string author = Console.ReadLine();
                            if (string.IsNullOrEmpty(author)) {
                                Console.WriteLine("Fältet var tomt, du måste ange ett namn."); //Avsluta applikation om fält är tomt
                                Environment.Exit(0);
                            }
                        Console.Write("Skriv ditt inlägg: ");
                            if (string.IsNullOrEmpty(author)) {
                                Console.WriteLine("Fltet var tomt, du måste skriva något."); //Avsluta applikation om fält är tomt
                                Environment.Exit(0);
                            }
                        string authorComment = Console.ReadLine();
                        Post newPost = new Post();
                        newPost.name = author;
                        newPost.comment = authorComment;
                        newApp.addPost(newPost);
                        break;
                    case "2": 
                        Console.CursorVisible = true;
                        Console.Write("Ange index att radera: ");
                        int index = Int32.Parse(Console.ReadLine()); //Konveretera console input till en int
                        if(index <= i){ 
                            newApp.delPost(index); //Radera inlägg och index är mindre eller lika med i
                            break;
                        } else {
                            Console.WriteLine("Inlägg finns inte, försök igen."); //Om index inte finns, avsluta applikation
                            Environment.Exit(0);
                        } 
                        break;
                    case "x": 
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Ett fel inträffade, försök igen."); //Om något annat än case 1, 2 och 3 matchas så avlustas applikationen
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}