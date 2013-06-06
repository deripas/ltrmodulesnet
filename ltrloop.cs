using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ltrModulesNet
{
    public class ltrloop
    {
        public const uint LTRLOOP_VERSION_1_0 = 0x01000000;
        public const uint LTRLOOP_VERSION = LTRLOOP_VERSION_1_0;
        public const int LTRLOOP_ID_MAX_SIZE = 32;

        public enum LTRLoopErr : int
        {
            LTRLOOP_ERR_NO_MEM = -1,
            LTRLOOP_ERR_XML_PARSER = -2,
            LTRLOOP_ERR_COMMUNICATION = -3,
            LTRLOOP_ERR_LOOP_VERSION = -4,
            LTRLOOP_ERR_INVAL_PARAM = -5,
            LTRLOOP_ERR_COMPILE_XML = -6
        };
        
        /** @brief  Состояния модуля */
        
        public enum LTRLoopState
        {
            /** @brief  Сброс (конфигурация не загружена) */
            LTRLOOP_STATE_RESET = 0,
            /** @brief  Ожидание (конфигурация загружена, но обработка не запущена) */
            LTRLOOP_STATE_IDLE = 1,
            /** @brief  Обработка */
            LTRLOOP_STATE_RUN = 2,
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ltrloop_descriptor_t
        {
            /** @brief  Размер стека данных в 16-битных словах */
            ushort _stack_size;
            /** @brief  Размер стека возвратов в 16-битных словах */
            ushort _rstack_size;
            /** @brief  Размер доступной памяти для словаря */
            ushort _mem_size;
            /** @brief  Размер выходного буфера в 32-битных словах */
            uint   _obuf_size;

            public ushort stack_size { get { return _stack_size; } }
            public ushort rstack_size { get { return _rstack_size; } }
            public ushort mem_size { get { return _mem_size; } }
            public uint obuf_size { get { return _obuf_size; } }
        };

        /** @brief  Статус модуля */
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ltrloop_status_t 
        {
            /** @brief  Состояние модуля */
            byte   _state;
            /** @brief  Код последней ошибки */
            ushort _last_error;
            /** @brief  ID конфигурации */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = LTRLOOP_ID_MAX_SIZE)]
            char[] _id;
            /** @brief  Время загрузки конфигурации (POSIX формат) */
            uint _load_tstamp;
            /** @brief  Время изменения состояния (POSIX формат) */
            uint _change_tstamp;

            public byte state { get { return _state; } }
            public ushort last_error { get { return _last_error; } }
            public string id { get { return new string(_id).TrimEnd('\0'); } }
            public uint load_tstamp { get { return _load_tstamp; } }
            public uint change_tstamp { get { return _change_tstamp; } }
        };


        /** @brief  Статистика модуля */
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ltrloop_stats_t
        {
            /** @brief  Размер использованной памяти */
            uint _mem_used;
            /** @brief  Максимальное время обработки */
            uint _max_proc_time;
            /** @brief  Количество запусков */
            uint _starts;
            /** @brief  Количество ошибок */
            uint _errors;
            /** @brief  Максимальная занятость буфера */
            uint _max_used_obuf;

            public uint mem_used { get { return _mem_used; } }
            public uint max_proc_time { get { return _max_proc_time; } }
            public uint starts { get { return _starts; } }
            public uint max_used_obuf { get { return _max_used_obuf; } }
        };


        /* Возвращает текущую версию библиотеки */
        [DllImport("ltrloop.dll")]
        public static extern uint ltrloop_get_version();

        /** @brief  Создает контекст модуля
         *
         *  Контекст модуля создается с учетом совместимости с версией библиотеки,
         *  с которой изначально компилировалось приложение. Полученный контекст
         *  используется практически во всех функциях библиотеки.
         *
         *  После завершения использования контекста, его необходимо уничтожить
         *  с помощью функции @ref ltrloop_delete.
         *
         *  @param  version Номер версии, с которым компилировалась программа
         *  @return Контекст модуля или NULL в случае ошибки, или если указанная
         *  версия библиотеки не поддерживается
         */
        [DllImport("ltrloop.dll")]
        public static extern IntPtr ltrloop_create(uint version);

        /** @brief  Уничтожает контекст модуля
         *
         *  Освобождает выделенные для хранения контекста модуля ресурсы. Если канал
         *  связи с крейт-контроллером не был закрыт, то он корректно закрывается.
         *  После уничтожения контекста модуля его дальнейшее использование
         *  недопустимо.
         *
         *  @param  loop    Контекст
         */
        [DllImport("ltrloop.dll")]
        public static extern void ltrloop_delete(IntPtr loop);

        /** @brief  Открывает канал связи с крейт-контроллером
         *
         *  Соединение осуществляется через LTR сервер, запущенный на хосте с
         *  IP адресом @a ip_addr и слушающий TCP порт @a port. Конкретный крейт
         *  выбирается по серийному номеру @a serial.
         *
         *  Если @a serial равен NULL или пустой строке, тогда открывается первый
         *  крейт-контроллер в списке LTR сервера. В качестве IP адреса и номера
         *  порта можно использовать константы SADDR_DEFAULT и SPORT_DEFAULT, что
         *  соответствует локальной машине и порту по умолчанию. В IP адресе байты
         *  идут от старшего к младшему (например, 127.0.0.1 соответствует
         *  0x7F000001UL).
         *
         *  После завершения работы с модулем крейт-контроллера необходимо закрыть
         *  канал связи при помощи функции @ref ltrloop_close.

         *  @param  loop    Контекст
         *  @param  ip_addr IP адрес хоста, на котором запущен LTR сервер
         *  @param  port    Порт, который прослушивается LTR сервером
         *  @param  serial  Серийный номер крейт-контроллера
         *  @return 0 или -1 в случае ошибки
         */
        [DllImport("ltrloop.dll")]
        public static extern int ltrloop_open(IntPtr loop, uint ip_addr, ushort port,
             string serial);

        /** @brief  Проверяет, открыт ли канал связи с крейт-контроллером
         *  @param  loop    Контекст
         *  @return Не 0, если канал связи закрыт, иначе -- 0.
         */
        [DllImport("ltrloop.dll")]
        public static extern byte ltrloop_is_open(IntPtr loop);

        /** @brief  Закрывает канал связи с крейт-контроллером
         *
         *  После окончания работы с модулем необходимо закрыть канал связи
         *  с крейт-контроллером.
         *
         *  @param  loop    Контекст
         *  @return 0 или -1 в случае ошибки
         */
        [DllImport("ltrloop.dll")]
        public static extern int ltrloop_close(IntPtr loop);

        /** @brief  Считывает описатель модуля
         *  @param  loop    Контекст
         *  @param  desc    Указатель на описатель
         *  @return 0 или -1 в случае ошибки
         */
        [DllImport("ltrloop.dll")]
        public static extern int ltrloop_get_descriptor(IntPtr loop, 
            out ltrloop_descriptor_t desc);

        /** @brief  Считывает статус модуля
         *  @param  loop    Контекст
         *  @param  status  Указатель на статус
         *  @return 0 или -1 в случае ошибки
         */
        [DllImport("ltrloop.dll")]
        public static extern int ltrloop_get_status(IntPtr loop, out ltrloop_status_t status);

        /** @brief  Считывает статистику модуля
         *  @param  loop    Контекст
         *  @param  stats   Указатель на статистику
         *  @return 0 или -1 в случае ошибки
         */
        [DllImport("ltrloop.dll")]
        public static extern int ltrloop_get_stats(IntPtr loop, out ltrloop_stats_t stats);

        /** @brief  Сбрасывает конфигурацию модуля
         *
         *  Сбрасывает конфигурацию модуля, обнуляет статистику, переводит модуль в
         *  состояние сброса (@ref LTRLOOP_STATE_RESET).
         *
         *  @param  loop    Контекст
         *  @return 0 или -1 в случае ошибки
         */
        [DllImport("ltrloop.dll")]
        public static extern int ltrloop_reset(IntPtr loop);

        /** @brief  Загружает конфигурацию из файла с описанием
         *  @param  loop    Контекст
         *  @param  file    Имя файла с описанием
         *  @return 0 или -1 в случае ошибки
         */
        [DllImport("ltrloop.dll")]
        public static extern int ltrloop_load_config_from_file(IntPtr loop, string file);

        /** @brief  Запускает обработку данных
         *  @param  loop    Контекст
         *  @return 0 или -1 в случае ошибки
         */
        [DllImport("ltrloop.dll")]
        public static extern int ltrloop_start(IntPtr loop);

        /** @brief  Останавливает обработку данных
         *  @param  loop    Контекст
         *  @return 0 или -1 в случае ошибки
         */
        [DllImport("ltrloop.dll")]
        public static extern int ltrloop_stop(IntPtr loop);

        /** @brief  Возвращает код последней ошибки
         *  @param  loop    Контекст
         *  @return Код последней ошибки
         */
        [DllImport("ltrloop.dll")]
        public static extern LTRLoopErr ltrloop_get_error(IntPtr loop);

        /** @brief  Возвращает текстовое описание последней ошибки
         *  @param  loop    Контекст
         *  @return Текстовое описание последней ошибки или NULL в случае ошибки
         */
        [DllImport("ltrloop.dll")]
        public static extern IntPtr ltrloop_get_error_message(IntPtr loop);



        public IntPtr hnd;

        public ltrloop()
        {
            hnd = ltrloop_create(LTRLOOP_VERSION);
        }

        ~ltrloop()
        {
            if (IsOpen())
                Close();
            ltrloop_delete(hnd);
        }

        public int Open(uint ip_addr, ushort port, string serial)
        {
            return ltrloop_open(hnd, ip_addr, port, serial);
        }

        public int Open(string serial)
        {
            return Open(_LTRNative.SADDR_DEFAULT, _LTRNative.SPORT_DEFAULT, serial);
        }

        public bool IsOpen()
        {
            return ltrloop_is_open(hnd)==0 ? false : true;
        }

        public int Close()
        {
            return ltrloop_close(hnd);
        }

        public int GetDescriptor(out ltrloop_descriptor_t desc)
        {
            return ltrloop_get_descriptor(hnd, out desc);
        }

        public int GetStatus(out ltrloop_status_t status)
        {
            return ltrloop_get_status(hnd, out status);
        }

        public int GetStats(out ltrloop_stats_t stats)
        {
            return ltrloop_get_stats(hnd, out stats);
        }

        public int Reset()
        {
            return ltrloop_reset(hnd);
        }

        public int LoadConfigFromFile(string file)
        {
            return ltrloop_load_config_from_file(hnd, file);
        }

        public int Start()
        {
            return ltrloop_start(hnd);
        }

        public int Stop()
        {
            return ltrloop_stop(hnd);
        }

        public LTRLoopErr GetError()
        {
            return ltrloop_get_error(hnd);
        }

        public string GetErrorMessage()
        {
            IntPtr ptr = ltrloop_get_error_message(hnd);
            string str = Marshal.PtrToStringAnsi(ptr);
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.UTF8;
            return dstEncodingFormat.GetString(Encoding.Convert(srcEncodingFormat, dstEncodingFormat, srcEncodingFormat.GetBytes(str)));
        }











    }
}
