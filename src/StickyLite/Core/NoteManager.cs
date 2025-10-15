using System.Collections.Concurrent;

namespace StickyLite.Core
{
    /// <summary>
    /// 다중 노트 관리자
    /// </summary>
    public static class NoteManager
    {
        private static readonly ConcurrentDictionary<string, MainForm> _activeNotes = new ConcurrentDictionary<string, MainForm>();
        private static readonly object _lockObject = new();

        /// <summary>
        /// 활성 노트 추가
        /// </summary>
        public static void AddNote(string noteId, MainForm note)
        {
            _activeNotes.TryAdd(noteId, note);
        }

        /// <summary>
        /// 활성 노트 제거
        /// </summary>
        public static void RemoveNote(string noteId)
        {
            _activeNotes.TryRemove(noteId, out _);
            
            // 마지막 노트가 닫히면 애플리케이션 종료
            if (_activeNotes.IsEmpty)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        /// <summary>
        /// 모든 활성 노트 가져오기
        /// </summary>
        public static IReadOnlyDictionary<string, MainForm> GetAllNotes()
        {
            return _activeNotes;
        }

        /// <summary>
        /// 특정 노트 가져오기
        /// </summary>
        public static MainForm? GetNote(string noteId)
        {
            _activeNotes.TryGetValue(noteId, out var note);
            return note;
        }

        /// <summary>
        /// 노트 개수
        /// </summary>
        public static int NoteCount => _activeNotes.Count;

        /// <summary>
        /// 모든 노트 닫기
        /// </summary>
        public static void CloseAllNotes()
        {
            lock (_lockObject)
            {
                foreach (var note in _activeNotes.Values.ToList())
                {
                    try
                    {
                        note.Close();
                    }
                    catch
                    {
                        // 노트가 이미 닫혔을 수 있음
                    }
                }
                _activeNotes.Clear();
            }
        }

        /// <summary>
        /// 특정 노트가 활성화되어 있는지 확인
        /// </summary>
        public static bool IsNoteActive(string noteId)
        {
            return _activeNotes.ContainsKey(noteId);
        }
    }
}
