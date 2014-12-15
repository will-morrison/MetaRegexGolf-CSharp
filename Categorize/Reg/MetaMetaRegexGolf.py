import re

def verify(regex, winners, losers):
    "Return true iff the regex matches all winners but no losers."
    missed_winners = {W for W in winners if not re.search(regex, W)}
    matched_losers = {L for L in losers if re.search(regex, L)}
    if missed_winners:
        print "Error: should match but did not:", ', '.join(missed_winners)
    if matched_losers:
        print "Error: should not match but did:", ', '.join(matched_losers)
    return not (missed_winners or matched_losers)

def findregex(winners, losers):
    "Find a regex that matches all winners but no losers (sets of strings)."
    # Make a pool of regex components, then pick from them to cover winners.
    # On each iteration, add the 'best' component to 'solution',
    # remove winners covered by best, and keep in 'pool' only components
    # that still match some winner.
    pool = regex_components(winners, losers)
    solution = []
    def score(r): return 4 * len(matches(r, winners)) - len(r)
    while winners:
        best = max(pool, key=score)
        solution.append(best)
        winners = winners - matches(best, winners)
        pool = {r for r in pool if matches(r, winners)}
    return OR(solution)

def matches(regex, strings):
    "Return a set of all the strings that are matched by regex."
    return {s for s in strings if re.search(regex, s)}

OR = '|'.join # Join a sequence of strings with '|' between them

def regex_components(winners, losers):
    "Return components that match at least one winner, but no loser."
    wholes = {'^'+winner+'$' for winner in winners}
    parts = {d for w in wholes for s in subparts(w) for d in dotify(s)}
    return wholes | {p for p in parts if not matches(p, losers)}

def subparts(word):
    "Return a set of subparts of word, consecutive characters up to length 4."
    return set(word[i:i+n] for i in range(len(word)) for n in (1, 2, 3, 4)) 

def dotify(part):
    "Return all ways to replace a subset of chars in part with '.'."
    if part == '':
        return {''}  
    else:
        return {c+rest for rest in dotify(part[1:]) 
                for c in replacements(part[0])}
       
def replacements(char):
    "Return replacement characters for char (char + '.' unless char is special)."
    if (char == '^' or char == '$'):
        return char
    else:
        return char + '.'
    
def words(text): return set(text.lower().split())

def lines(text): return {line.upper().strip() for line in text.split('/')}

################ Data

winners = words('''washington adams jefferson jefferson madison madison monroe 
    monroe adams jackson jackson van-buren harrison polk taylor pierce buchanan 
    lincoln lincoln grant grant hayes garfield cleveland harrison cleveland mckinley
    mckinley roosevelt taft wilson wilson harding coolidge hoover roosevelt 
    roosevelt roosevelt roosevelt truman eisenhower eisenhower kennedy johnson nixon 
    nixon carter reagan reagan bush clinton clinton bush bush obama obama''')

losers = words('''clinton jefferson adams pinckney pinckney clinton king adams 
    jackson adams clay van-buren van-buren clay cass scott fremont breckinridge 
    mcclellan seymour greeley tilden hancock blaine cleveland harrison bryan bryan 
    parker bryan roosevelt hughes cox davis smith hoover landon wilkie dewey dewey 
    stevenson stevenson nixon goldwater humphrey mcgovern ford carter mondale 
    dukakis bush dole gore kerry mccain romney''')

starwars = lines('''The Phantom Menace / Attack of the Clones / Revenge of the Sith /
    A New Hope / The Empire Strikes Back / Return of the Jedi''')

startrek = lines('''The Wrath of Khan / The Search for Spock / The Voyage Home /
    The Final Frontier / The Undiscovered Country / Generations / First Contact /
    Insurrection / Nemesis''')

def findboth(A, B):
    "Find a regex to match A but not B, and vice-versa.  Print summary."
    for (W, L, legend) in [(A, B, 'A-B'), (B, A, 'B-A')]:
        solution = findregex(W, L)
        assert verify(solution, W, L)
        ratio = len('^(' + OR(W) + ')$') / float(len(solution))
        print '%3d chars, %4.1f ratio, %2d winners %s: %r' % (
            len(solution), ratio , len(W), legend, solution)
        
################ Tests
        
def test1():
    assert subparts('^it$') == {'^', 'i', 't', '$', '^i', 'it', 't$', '^it', 'it$', '^it$'}
    assert subparts('this') == {'t', 'h', 'i', 's', 'th', 'hi', 'is', 'thi', 'his', 'this'}
    assert dotify('it') == {'it', 'i.', '.t', '..'}
    assert dotify('^it$') == {'^it$', '^i.$', '^.t$', '^..$'}
    assert dotify('this') == {'this', 'thi.', 'th.s', 'th..', 't.is', 't.i.', 't..s', 't...',
                              '.his', '.hi.', '.h.s', '.h..', '..is', '..i.', '...s', '....'}
    assert replacements('x') == 'x.'
    assert replacements('^') == '^'
    assert replacements('$') == '$'
    assert regex_components({'win'}, {'losers', 'bin', 'won'}) == {
        '^win$', '^win', '^wi.', 'wi.',  'wi', '^wi', 'win$', 'win', 'wi.$'}
    assert regex_components({'win'}, {'losers', 'bin', 'won', 'wine'}) == {
        '^win$', 'win$', 'wi.$'}
    assert matches('a|b|c', {'a', 'b', 'c', 'd', 'e'}) == {'a', 'b', 'c'}
    assert matches('a|b|c', {'any', 'bee', 'succeed', 'dee', 'eee!'}) == {
        'any', 'bee', 'succeed'}
    assert verify('a+b+', {'ab', 'aaabb'}, {'a', 'bee', 'a b'})
    assert findregex({"ahahah", "ciao"},  {"ahaha", "bye"}) == 'a.$' 
    assert OR(['a', 'b', 'c']) == 'a|b|c'
    assert OR(['a']) == 'a'
    assert words('This is a TEST this is') == {'this', 'is', 'a', 'test'}
    assert lines('Testing / 1 2 3 / Testing over') == {'TESTING', '1 2 3', 'TESTING OVER'}
    return 'test1 passes'

if __name__ == '__main__':
    print test1()