using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*21.	Реализация режима обратной связи по шифртексту AES.*/

namespace KZI
{
    class AES
    {
        private int Nb = 4;//byte
        private int Nk { get; set; }//byte
        private int Nr { get; set; }//byte
        private string Key { get; set; }

        private string[,] Sbox { get; set; } = {
                {"63", "7C", "77", "7B", "F2", "6B", "6F", "C5", "30", "01", "67", "2B", "FE", "D7", "AB", "76" },
                {"CA", "82", "C9", "7D", "FA", "59", "47", "F0", "AD", "D4", "A2", "AF", "9C", "A4", "72", "C0" },
                { "B7", "FD", "93", "26", "36", "3F", "F7", "CC", "34", "A5", "E5", "F1", "71", "D8", "31", "15" },
                { "04", "C7", "23", "C3", "18", "96", "05", "9A", "07", "12", "80", "E2", "EB", "27", "B2", "75" },
                { "09", "83", "2C", "1A", "1B", "6E", "5A", "A0", "52", "3B", "D6", "B3", "29", "E3", "2F", "84" },
                { "53", "D1", "00", "ED", "20", "FC", "B1", "5B", "6A", "CB", "BE", "39", "4A", "4C", "58", "CF", },
                { "D0", "EF", "AA", "FB", "43", "4D", "33", "85", "45", "F9", "02", "7F", "50", "3C", "9F", "A8" },
                { "51", "A3", "40", "8F", "92", "9D", "38", "F5", "BC", "B6", "DA", "21", "10", "FF", "F3", "D2" },
                { "CD", "0C", "13", "EC", "5F", "97", "44", "17", "C4", "A7", "7E", "3D", "64", "5D", "19", "73" },
                { "60", "81", "4F", "DC", "22", "2A", "90", "88", "46", "EE", "B8", "14", "DE", "5E", "0B", "DB" },
                { "E0", "32", "3A", "0A", "49", "06", "24", "5C", "C2", "D3", "AC", "62", "91", "95", "E4", "79" },
                { "E7", "C8", "37", "6D", "8D", "D5", "4E", "A9", "6C", "56", "F4", "EA", "65", "7A", "AE", "08" },
                { "BA", "78", "25", "2E", "1C", "A6", "B4", "C6", "E8", "DD", "74", "1F", "4B", "BD", "8B", "8A" },
                { "70", "3E", "B5", "66", "48", "03", "F6", "0E", "61", "35", "57", "B9", "86", "C1", "1D", "9E" },
                { "E1", "F8", "98", "11", "69", "D9", "8E", "94", "9B", "1E", "87", "E9", "CE", "55", "28", "DF" },
                { "8C", "A1", "89", "0D", "BF", "E6", "42", "68", "41", "99", "2D", "0F", "B0", "54", "BB", "16" }
            };


        private string[] Rcon { get; set; } =
        {
            "01","02","04","08","10","20","40","80","1b","36",
            "00","00","00","00","00","00","00","00","00","00",
            "00","00","00","00","00","00","00","00","00","00",
            "00","00","00","00","00","00","00","00","00","00"
        };


        public AES(string key_path)
        {
            // Nk = key_length;
            Nk = GetKey(key_path);
            switch (Nk)
            {
                case 4:
                    {
                        Nr = 10;
                    }
                    break;
                case 6:
                    {
                        Nr = 12;
                    }
                    break;
                case 8:
                    {
                        Nr = 14;
                    }
                    break;
                default:
                    {
                        throw new Exception("Неправильный размер ключа");
                    }
                    break;
            }


        }

        private byte Hex2Byte(string hex)
        {
            byte b = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return b;
        }

        private string Byte2Hex(byte b)
        {
            string hex = "";
            byte hiword = (byte)(b >> 4);
            byte loword = (byte)(b & 15);
            if (hiword < 10) hex += hiword;
            else
            {
                switch (hiword)
                {
                    case 10: hex += "A"; break;
                    case 11: hex += "B"; break;
                    case 12: hex += "C"; break;
                    case 13: hex += "D"; break;
                    case 14: hex += "E"; break;
                    case 15: hex += "F"; break;
                }
            }
            if (loword < 10) hex += loword;
            else
            {
                switch (loword)
                {
                    case 10: hex += "A"; break;
                    case 11: hex += "B"; break;
                    case 12: hex += "C"; break;
                    case 13: hex += "D"; break;
                    case 14: hex += "E"; break;
                    case 15: hex += "F"; break;
                }
            }
            return hex;
        }

        private string read_file(string path)
        {
            StreamReader sr = new StreamReader(path, false);
            string text = sr.ReadToEnd();
            sr.Close();
            return text;
        }

        private void save_file(string path, string str)
        {
            StreamWriter file = new StreamWriter(path, false);
            file.Write(str);

            file.Close();
        }

        private int GetKey(string path)
        {
            Key = read_file(path);
            byte[] keyb = Encoding.Default.GetBytes(Key);
            int kn = keyb.Length / 4;
            return kn;
        }

        private byte[] KeyExpansion(byte[] key)
        {
            int i = 0;
            byte[] w = new byte[(Nb * (Nr + 1)) * 4];
            while (i < Nk)
            {
                w[4 * i] = key[4 * i];
                w[(4 * i) + 1] = key[(4 * i) + 1];
                w[(4 * i) + 2] = key[(4 * i) + 2];
                w[(4 * i) + 3] = key[(4 * i) + 3];
                i++;
            }

            i = Nk;
            while (i < Nb * (Nr + 1))
            {
                byte[] temp = { w[4 * (i - 1)], w[(4 * (i - 1)) + 1], w[(4 * (i - 1)) + 2], w[(4 * (i - 1)) + 3] };
                if (i % Nk == 0)
                {
                    byte[] t = SubWord(Rotword(temp));
                    temp[0] = (byte)(t[0] ^ Hex2Byte(Rcon[i / Nk]));
                    temp[1] = (byte)(t[1] ^ Hex2Byte(Rcon[i / Nk]));
                    temp[2] = (byte)(t[2] ^ Hex2Byte(Rcon[i / Nk]));
                    temp[3] = (byte)(t[3] ^ Hex2Byte(Rcon[i / Nk]));
                }
                else if ((Nk > 6) && (i % Nk == 4))
                {
                    temp = SubWord(temp);
                }

                w[4 * i] = (byte)(w[i - Nk] ^ temp[0]);
                w[(4 * i) + 1] = (byte)(w[i - Nk + 1] ^ temp[1]);
                w[(4 * i) + 2] = (byte)(w[i - Nk + 2] ^ temp[2]);
                w[(4 * i) + 3] = (byte)(w[i - Nk + 3] ^ temp[3]);
                i++;
            }
            return w;
        }

        private byte[] SubWord(byte[] w)
        {
            string[] hex_tmp = new string[w.Length];

            for (int i = 0; i < w.Length; i++)
            {
                int hiword = (w[i] >> 4);
                int loword = (w[i] & 15);
                hex_tmp[i] = Sbox[hiword, loword];
            }

            byte[] tmp = new byte[4];
            for (int i = 0; i < hex_tmp.Length; i++)
            {
                tmp[i] = Hex2Byte(hex_tmp[i]);
            }

            return tmp;
        }

        private byte[] Rotword(byte[] w)
        {
            string[] hex_tmp = new string[w.Length];
            for (int i = 0; i < hex_tmp.Length; i++)
            {
                hex_tmp[i] = Byte2Hex(w[i]);
            }

            string[] tmp = shift_left_array(hex_tmp, 1);

            byte[] b = new byte[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                b[i] = Hex2Byte(tmp[i]);
            }

            return b;
        }
        public string Encode(string path)
        {
            string open_text = read_file(path);
            string crypt_text = "";


            int len = open_text.Length % 16;
            if (len != 0)
            {
                len = 16 - len;
                while (len > 0)
                {
                    open_text += " ";
                    len--;
                }
            }
            byte[] text = Encoding.Default.GetBytes(open_text);
            int count_block = text.Length / 16;

            byte[] key = Encoding.Default.GetBytes(Key);


            byte[] w = KeyExpansion(key);

            for (int i = 0; i < count_block; i++)
            {
                string[] inp = new string[16];
                int count = 0;
                for (int k = 16 * i; k < (16 * i) + 16; k++)
                {
                    inp[count] = Byte2Hex(text[k]);
                    count++;
                }
                string[,] input_block = new string[Nb, Nb];                
                for (int r = 0; r < input_block.Length / Nb; r++)
                {
                    for (int c = 0; c < input_block.Length / Nb; c++)
                    {                        
                        input_block[r, c] = inp[r + (4 * c)];                        
                    }
                }

                string[] out_block = Cipher(input_block, w);

                byte[] b = new byte[16];
                for (int j = 0; j < 16; j++)
                {
                    b[j] = Hex2Byte(out_block[j]);
                }

                crypt_text += Encoding.Default.GetString(b);               
            }

            SaveFileDialog file = new SaveFileDialog();
            file.ShowDialog();
            save_file(file.FileName, crypt_text);
            return crypt_text;
        }


        public string EncodeCFB(string path, string iv_path, bool decode = false)
        {
            string open_text = read_file(path);
            string iv_text = read_file(iv_path);
            string crypt_text = "";


            int len = open_text.Length % 16;
            if (len != 0)
            {
                len = 16 - len;
                while (len > 0)
                {
                    open_text += " ";
                    len--;
                }
            }
            byte[] text = Encoding.Default.GetBytes(open_text);
            int count_block = text.Length / 16;

            byte[] iv_byte = Encoding.Default.GetBytes(iv_text);

            byte[] key = Encoding.Default.GetBytes(Key);


            byte[] w = KeyExpansion(key);
            byte[] li = new byte[1];
            for (int i = 0; i < count_block; i++)
            {
                
                string[] inp = new string[16];
                int count = 0;
                for (int k = 16 * i; k < (16 * i) + 16; k++)
                {
                    inp[count] = Byte2Hex(text[k]);
                    count++;
                }                

                int count_j = 8;                
                string[] out_block = new string[16];
                
                for (int j = 0; j < 128 / count_j; j++)
                {                   

                    if ((i == 0) && (j==0))
                    {
                        li = iv_byte;   
                    }                    

                    string[] inp_b = new string[16];                    
                    for (int k = 0; k < 16; k++)
                    {
                        inp_b[k] = Byte2Hex(li[k]);
                       
                    }
                    string[,] input_block_b = new string[Nb, Nb];
                    for (int r = 0; r < input_block_b.Length / Nb; r++)
                    {
                        for (int c = 0; c < input_block_b.Length / Nb; c++)
                        {
                            input_block_b[r, c] = inp_b[r + (4 * c)];
                        }
                    }

                    string[] Oi = Cipher(input_block_b, w);
                    string Ci = AddPortionOfText(inp[j], Oi[0]);
                    Oi = shift_left_array(Oi, count_j / 8);
                    
                    byte[] tmp = li;
                    tmp = shift_left_array(tmp, 1);
                    if(!decode) tmp[tmp.Length - 1] = Hex2Byte(Ci);         
                    else tmp[tmp.Length - 1] = Hex2Byte(inp[j]);
                    li = tmp;                    
                    
                    out_block[j] = Ci;                    
                }

                byte[] b = new byte[16];
                for (int j = 0; j < 16; j++)
                {
                    b[j] = Hex2Byte(out_block[j]);
                }

                crypt_text += Encoding.Default.GetString(b);
            }

            SaveFileDialog file = new SaveFileDialog();
            file.ShowDialog();
            save_file(file.FileName, crypt_text);
            return crypt_text;
        }

        public string DecodeCFB(string path, string iv_path)
        {
            string crypt_text = read_file(path);
            string iv_text = read_file(iv_path);
            string open_text = "";
                                   
            byte[] text = Encoding.Default.GetBytes(crypt_text);
            int count_block = text.Length / 16;

            byte[] iv_byte = Encoding.Default.GetBytes(iv_text);

            byte[] key = Encoding.Default.GetBytes(Key);


            byte[] w = KeyExpansion(key);
            byte[] li = new byte[1];
            for (int i = 0; i < count_block; i++)
            {

                string[] inp = new string[16];
                int count = 0;
                for (int k = 16 * i; k < (16 * i) + 16; k++)
                {
                    inp[count] = Byte2Hex(text[k]);
                    count++;
                }

                int count_j = 8;
                string[] out_block = new string[16];

                for (int j = 0; j < 128 / count_j; j++)
                {

                    if ((i == 0) && (j == 0))
                    {
                        li = iv_byte;
                    }

                    string[] inp_b = new string[16];
                    for (int k = 0; k < 16; k++)
                    {
                        inp_b[k] = Byte2Hex(li[k]);

                    }
                    string[,] input_block_b = new string[Nb, Nb];
                    for (int r = 0; r < input_block_b.Length / Nb; r++)
                    {
                        for (int c = 0; c < input_block_b.Length / Nb; c++)
                        {
                            input_block_b[r, c] = inp_b[r + (4 * c)];
                        }
                    }

                    string[] Oi = Cipher(input_block_b, w);
                    string Pi = AddPortionOfText(inp[j], Oi[0]);
                    Oi = shift_left_array(Oi, count_j / 8);

                    byte[] tmp = li;
                    tmp = shift_left_array(tmp, 1);
                    tmp[tmp.Length - 1] = Hex2Byte(inp[j]);
                    li = tmp;

                    out_block[j] = Pi;
                }

                byte[] b = new byte[16];
                for (int j = 0; j < 16; j++)
                {
                    b[j] = Hex2Byte(out_block[j]);
                }

                open_text += Encoding.Default.GetString(b);
            }

            SaveFileDialog file = new SaveFileDialog();
            file.ShowDialog();
            save_file(file.FileName, open_text);
            return open_text;
        }

        private string AddPortionOfText(string Pi, string St)
        {
            byte tmp = (byte)(Hex2Byte(Pi) ^ Hex2Byte(St));
            return Byte2Hex(tmp);
        } 


        /// <summary>
        /// Суммирование по модулю 2 с начальным ключом шифра
        /// </summary>
        private string[,] AddRoundKey(string[,] state, string[,] round_key)
        {
            byte[,] s = new byte[Nb, Nb];
            for (int i = 0; i < Nb; i++)
            {
                for (int j = 0; j < Nb; j++)
                {
                    s[i, j] = Hex2Byte(state[i, j]);
                }
            }

            byte[,] rk = new byte[Nb, Nb];
            for (int i = 0; i < Nb; i++)
            {
                for (int j = 0; j < Nb; j++)
                {
                    rk[i, j] = Hex2Byte(round_key[i, j]);
                }
            }

            byte[,] tmp = new byte[Nb, Nb];

            for (int i = 0; i < Nb; i++)
            {
                for (int j = 0; j < Nb; j++)
                {
                    tmp[i, j] = (byte)(s[i, j] ^ rk[i, j]);
                }
            }

            string[,] tmps = new string[Nb, Nb];
            for (int i = 0; i < Nb; i++)
            {
                for (int j = 0; j < Nb; j++)
                {
                    tmps[i, j] = Byte2Hex(tmp[i, j]);
                }
            }

            return tmps;
        }

        private string[] Cipher(string[,] input_block, byte[] w)
        {
            string[,] state = input_block;
            string[,] w0 = new string[4, 4] {
                    { Byte2Hex(w[0]),Byte2Hex(w[1]),Byte2Hex(w[2]),Byte2Hex(w[3])},
                    { Byte2Hex(w[4]),Byte2Hex(w[5]),Byte2Hex(w[6]),Byte2Hex(w[7])},
                    { Byte2Hex(w[8]),Byte2Hex(w[9]),Byte2Hex(w[10]),Byte2Hex(w[11])},
                    { Byte2Hex(w[12]),Byte2Hex(w[13]),Byte2Hex(w[14]),Byte2Hex(w[15])}
                };


            state = AddRoundKey(state, w0);

            for (int round = 1; round <= Nr - 1; round++)
            {
                string[,] wi = new string[4, 4] {
                        { Byte2Hex(w[round*16]),Byte2Hex(w[(round*16)+1]),Byte2Hex(w[(round*16)+2]),Byte2Hex(w[(round*16)+3])},
                        { Byte2Hex(w[(round*16)+4]),Byte2Hex(w[(round*16)+5]),Byte2Hex(w[(round*16)+6]),Byte2Hex(w[(round*16)+7])},
                        { Byte2Hex(w[(round*16)+8]),Byte2Hex(w[(round*16)+9]),Byte2Hex(w[(round*16)+10]),Byte2Hex(w[(round*16)+11])},
                        { Byte2Hex(w[(round*16)+12]),Byte2Hex(w[(round*16)+13]),Byte2Hex(w[(round*16)+14]),Byte2Hex(w[(round*16)+15])}
                     };
                state = Round(state, wi);

            }

            state = SubBytes(state);
            state = ShiftRows(state);
            string[,] we = new string[4, 4] {
                    { Byte2Hex(w[Nr*16]),Byte2Hex(w[(Nr*16)+1]),Byte2Hex(w[(Nr*16)+2]),Byte2Hex(w[(Nr*16)+3])},
                        { Byte2Hex(w[(Nr*16)+4]),Byte2Hex(w[(Nr*16)+5]),Byte2Hex(w[(Nr*16)+6]),Byte2Hex(w[(Nr*16)+7])},
                        { Byte2Hex(w[(Nr*16)+8]),Byte2Hex(w[(Nr*16)+9]),Byte2Hex(w[(Nr*16)+10]),Byte2Hex(w[(Nr*16)+11])},
                        { Byte2Hex(w[(Nr*16)+12]),Byte2Hex(w[(Nr*16)+13]),Byte2Hex(w[(Nr*16)+14]),Byte2Hex(w[(Nr*16)+15])}
                };
            state = AddRoundKey(state, we);

            // string[,] out_block = state;
            string[] out_block = new string[Nb * Nb];
            for (int r = 0; r < Nb; r++)
            {
                for (int c = 0; c < Nb; c++)
                {
                    out_block[r + (4 * c)] = state[r, c];
                }
            }         

            return out_block;
        }

        private string[,] Round(string[,] state, string[,] round_key)
        {
            state = SubBytes(state);
            state = ShiftRows(state);
            state = MixColumns(state);
            state = AddRoundKey(state, round_key);

            return state;
        }


        /// <summary>
        /// побайтовая подстановка в S-боксе с фиксированной таблицей замен;
        /// </summary>
        private string[,] SubBytes(string[,] state)
        {
            string[,] tmp = new string[Nb, Nb];

            for (int i = 0; i < Nb; i++)
            {
                for (int j = 0; j < Nb; j++)
                {
                    byte k = Hex2Byte(state[i, j]);
                    int hiword = (k >> 4);
                    int loword = (k & 15);
                    tmp[i, j] = Sbox[hiword, loword];

                }
            }

            return tmp;
        }

        public string[] shift_right_array(string[] w, int r)
        {
            string[] tmp = new string[w.Length];
            for (int i = tmp.Length - 1; i >= 0; i--)
            {
                tmp[(i + r) % tmp.Length] = w[i];
            }

            return tmp;
        }

        public byte[] shift_right_array(byte[] w, int r)
        {
            byte[] tmp = new byte[w.Length];
            for (int i = tmp.Length - 1; i >= 0; i--)
            {
                tmp[(i + r) % tmp.Length] = w[i];
            }

            return tmp;
        }

        public string[] shift_left_array(string[] w, int r)
        {
            string[] tmp = new string[w.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                int h = (i - r) % tmp.Length;
                if (h < 0) h += tmp.Length;
                tmp[h] = w[i];
            }

            return tmp;
        }

        public byte[] shift_left_array(byte[] w, int r)
        {
            byte[] tmp = new byte[w.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                int h = (i - r) % tmp.Length;
                if (h < 0) h += tmp.Length;
                tmp[h] = w[i];
            }

            return tmp;
        }

        /// <summary>
        /// побайтовый сдвиг строк матрицы State на различное количество байт;
        /// </summary>
        private string[,] ShiftRows(string[,] state)
        {
            string[,] tmp = new string[Nb, Nb];
            for (int i = 0; i < Nb; i++)
            {
                string[] shift_tmp = new string[Nb];
                for (int j = 0; j < Nb; j++)
                {
                    shift_tmp[j] = state[i, j];
                }
                if ((Nk == 8) && (i >= 2))
                {
                    shift_tmp = shift_left_array(shift_tmp, i + 1);

                }
                else
                {
                    shift_tmp = shift_left_array(shift_tmp, i);

                }

                for (int j = 0; j < Nb; j++)
                {
                    tmp[i, j] = shift_tmp[j];
                }
            }

            return tmp;
        }

        private byte Multi_hex_02(byte num)
        {
            byte tmp;
            if (num < Hex2Byte("80"))
            {
                tmp = (byte)(num << 1);
            }
            else
            {
                tmp = (byte)((num << 1) ^ Hex2Byte("1B"));
            }

            return (byte)(tmp % 0x100);
        }
        private byte Multi_hex_03(byte num)
        {
            return (byte)(Multi_hex_02(num) ^ num);
        }

        private byte[] MultiplicationGalua(byte[] column)
        {
            byte[] tmp = new byte[Nb];

            tmp[0] = (byte)(Multi_hex_02(column[0]) ^ Multi_hex_03(column[1]) ^ column[2] ^ column[3]);
            tmp[1] = (byte)(column[0] ^ Multi_hex_02(column[1]) ^ Multi_hex_03(column[2]) ^ column[3]);
            tmp[2] = (byte)(column[0] ^ column[1] ^ Multi_hex_02(column[2]) ^ Multi_hex_03(column[3]));
            tmp[3] = (byte)(Multi_hex_03(column[0]) ^ column[1] ^ column[2] ^ Multi_hex_02(column[3]));

            return tmp;
        }
        /// <summary>
        /// перемешивание байт в столбцах;
        /// </summary>
        private string[,] MixColumns(string[,] state)
        {
            byte[,] s = new byte[Nb, Nb];
            for (int i = 0; i < Nb; i++)
            {
                for (int j = 0; j < Nb; j++)
                {
                    s[i, j] = Hex2Byte(state[i, j]);
                }
            }

            byte[,] tmp = new byte[Nb, Nb];
            for (int i = 0; i < Nb; i++)
            {
                byte[] m = new byte[4];

                for (int j = 0; j < Nb; j++)
                {
                    m[j] = s[j, i];
                }

                byte[] k = new byte[Nb];
                k = MultiplicationGalua(m);


                for (int j = 0; j < Nb; j++)
                {
                    tmp[j, i] = k[j];
                }
            }

            string[,] htmp = new string[Nb, Nb];
            for (int i = 0; i < Nb; i++)
            {
                for (int j = 0; j < Nb; j++)
                {
                    htmp[i, j] = Byte2Hex(tmp[i, j]);
                }
            }

            return htmp;
        }

    }
}
